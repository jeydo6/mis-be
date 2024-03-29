﻿using System;
using System.Linq;
using Microsoft.Extensions.Options;
using MIS.Be.Application.Configs;
using MIS.Be.Application.ViewModels;
using MIS.Be.Domain.Extensions;
using MIS.Be.Domain.Providers;
using MIS.Be.Domain.Repositories;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Queries
{
	public class SpecialtyListItemsHandler : IRequestHandler<SpecialtyListItemsQuery, SpecialtyViewModel[]>
	{
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly IResourcesRepository _resources;
		private readonly ITimeItemsRepository _timeItems;

		private readonly SettingsConfig _settingsConfig;

		public SpecialtyListItemsHandler(
			IDateTimeProvider dateTimeProvider,
			IResourcesRepository resources,
			ITimeItemsRepository timeItems,
			IOptionsSnapshot<SettingsConfig> settingsConfigOptions
		)
		{
			_dateTimeProvider = dateTimeProvider;
			_resources = resources;
			_timeItems = timeItems;
			_settingsConfig = settingsConfigOptions.Value;
		}

		public SpecialtyViewModel[] Handle(SpecialtyListItemsQuery request)
		{
			var visitItems = request.Patient != null ? request.Patient.VisitItems
				.ToArray() : Array.Empty<VisitItemViewModel>();

			var dispanserizations = request.Patient != null ? request.Patient.Dispanserizations
				.ToArray() : Array.Empty<DispanserizationViewModel>();

			var beginDate = _dateTimeProvider.Now.Date;
			var endDate = _dateTimeProvider.Now.Date.AddDays(28);

			var resources = _resources.ToList();
			var resourceTotals = _timeItems.GetResourceTotals(beginDate, endDate);

			var dateItems = resourceTotals
				.GroupJoin(visitItems, t => t.ResourceID, g => g.ResourceID, (t, g) => new DateItemViewModel
				{
					Date = t.Date,
					BeginDateTime = t.BeginDateTime,
					EndDateTime = t.EndDateTime,
					Count = t.TimesCount - t.VisitsCount,
					IsEnabled = (t.TimesCount - t.VisitsCount) > 0,
					IsBlocked = g.Any(),
					ResourceID = t.ResourceID
				})
				.OrderBy(di => di.Date)
				.ToArray();

			var resourceItems = resources
				.GroupJoin(dateItems, r => r.ID, d => d.ResourceID, (r, g) => new ResourceViewModel
				{
					EmployeeName = r.Employee.GetName(),
					RoomCode = r.Room.Code,
					Count = g.Sum(di => di.Count),
					IsEnabled = g.Any(di => di.IsEnabled) && g.All(di => !di.IsBlocked),
					IsBlocked = g.Any(di => di.IsBlocked),
					ResourceID = r.ID,
					SpecialtyID = r.Employee.SpecialtyID,
					Dates = g.ToArray()
				})
				.OrderBy(ri => ri.EmployeeName)
				.ToArray();

			var specialtyItems = resources
				.GroupBy(r => new { r.Employee.Specialty.ID, r.Employee.Specialty.Name })
				.Select(g => g.Key)
				.GroupJoin(resourceItems, s => s.ID, g => g.SpecialtyID, (s, g) => new SpecialtyViewModel
				{
					SpecialtyName = s.Name,
					Count = g.Sum(di => di.Count),
					IsEnabled = g.Any(ri => ri.IsEnabled) && g.All(ri => !ri.IsBlocked),
					SpecialtyID = s.ID,
					Resources = g.ToArray()
				})
				.OrderBy(si => si.SpecialtyName)
				.ToList();

			var dispanserizationSpecialtyItem = specialtyItems.FirstOrDefault(s => s.SpecialtyName == "Диспансеризация");
			if (dispanserizationSpecialtyItem != null)
			{
				var dispanserization = dispanserizations
					.OrderBy(d => d.BeginDate)
					.LastOrDefault(d => !d.IsClosed && d.BeginDate.Year == _dateTimeProvider.Now.Year);

				if (dispanserization != null)
				{
					foreach (var ri in dispanserizationSpecialtyItem.Resources)
					{
						var dispanserizationBeginDate = dispanserization.BeginDate;
						var dispanserizationEndDate = _settingsConfig.DispanserizationInterval.HasValue ?
							dispanserization.BeginDate.AddDays(_settingsConfig.DispanserizationInterval.Value) :
							endDate;

						ri.Dates = ri.Dates.Where(di => di.Date >= dispanserizationBeginDate && di.Date < dispanserizationEndDate).ToArray();
						ri.IsEnabled = ri.Dates.Any(di => di.IsEnabled) && ri.Dates.All(di => !di.IsBlocked);
						ri.IsBlocked = ri.Dates.Any(di => di.IsBlocked);
					}
					dispanserizationSpecialtyItem.IsEnabled = dispanserizationSpecialtyItem.Resources.Any(ri => ri.IsEnabled);
				}
				else
				{
					specialtyItems.Remove(dispanserizationSpecialtyItem);
				}
			}

			var result = specialtyItems
				.ToArray();

			return result;
		}
	}
}
