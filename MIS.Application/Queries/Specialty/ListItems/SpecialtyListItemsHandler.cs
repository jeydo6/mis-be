#region Copyright © 2020 Vladimir Deryagin. All rights reserved
/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

using MediatR;
using MIS.Application.ViewModels;
using MIS.Domain.Entities;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Queries
{
	public class SpecialtyListItemsHandler : IRequestHandler<SpecialtyListItemsQuery, IEnumerable<SpecialtyViewModel>>
	{
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly IResourcesRepository _resources;
		private readonly ITimeItemsRepository _timeItems;

		public SpecialtyListItemsHandler(
			IDateTimeProvider dateTimeProvider,
			IResourcesRepository resources,
			ITimeItemsRepository timeItems
		)
		{
			_dateTimeProvider = dateTimeProvider;
			_resources = resources;
			_timeItems = timeItems;
		}

		public async Task<IEnumerable<SpecialtyViewModel>> Handle(SpecialtyListItemsQuery request, CancellationToken cancellationToken)
		{
			DateTime beginDate = _dateTimeProvider.Now.Date;
			DateTime endDate = _dateTimeProvider.Now.Date.AddDays(28);

			IEnumerable<Resource> resources = _resources.ToList();
			IEnumerable<TimeItemTotal> totals = _timeItems.GetResourceTotals(beginDate, endDate);

			IEnumerable<VisitItemViewModel> visitItems = request.Patient.VisitItems
				.ToList();

			IEnumerable<DateItemViewModel> dateItems = totals
				.GroupJoin(visitItems, t => t.ResourceID, g => g.ResourceID, (t, g) => new DateItemViewModel
				{
					Date = t.Date,
					IsEnabled = (t.TimesCount - t.VisitsCount) > 0,
					IsBlocked = g.Any(),
					ResourceID = t.ResourceID
				})
				.OrderBy(di => di.Date)
				.ToList();

			IEnumerable<ResourceViewModel> resourceItems = resources
				.GroupJoin(dateItems, r => r.ID, d => d.ResourceID, (r, g) => new ResourceViewModel
				{
					ResourceName = r.Doctor.DisplayName,
					IsEnabled = g.Any(di => di.IsEnabled) && g.All(di => !di.IsBlocked),
					IsBlocked = g.Any(di => di.IsBlocked),
					ResourceID = r.ID,
					SpecialtyID = r.Doctor.SpecialtyID,
					Dates = g
				})
				.OrderBy(ri => ri.ResourceName)
				.ToList();

			ICollection<SpecialtyViewModel> viewModels = resources
				.GroupBy(r => r.Doctor.Specialty)
				.Select(g => g.Key)
				.GroupJoin(resourceItems, s => s.ID, g => g.SpecialtyID, (s, g) => new SpecialtyViewModel
				{
					SpecialtyName = s.Name,
					Resources = g,
					IsEnabled = g.Any(ri => ri.IsEnabled) && g.All(ri => !ri.IsBlocked)
				})
				.OrderBy(si => si.SpecialtyName)
				.ToList();

			SpecialtyViewModel dispanserizationViewModel = viewModels.FirstOrDefault(s => s.SpecialtyName == "Диспансеризация");
			if (dispanserizationViewModel != null)
			{
				DispanserizationViewModel dispanserization = request.Patient.Dispanserizations
					.OrderBy(d => d.BeginDate)
					.LastOrDefault(d => !d.IsClosed && d.BeginDate.Year == _dateTimeProvider.Now.Year);

				if (dispanserization != null)
				{
					foreach (var ri in dispanserizationViewModel.Resources)
					{
						ri.Dates = ri.Dates.Where(di => di.Date >= dispanserization.BeginDate);
						ri.IsEnabled = ri.Dates.Any(di => di.IsEnabled) && ri.Dates.All(di => !di.IsBlocked);
						ri.IsBlocked = ri.Dates.Any(di => di.IsBlocked);
					}
					dispanserizationViewModel.IsEnabled = dispanserizationViewModel.Resources.Any(ri => ri.IsEnabled);
				}
				else
				{
					viewModels.Remove(dispanserizationViewModel);
				}
			}

			return await Task.FromResult(viewModels);
		}
	}
}
