using System;
using System.Collections.Generic;
using System.Linq;
using MIS.Demo.DataContexts;
using MIS.Domain.Entities;
using MIS.Domain.Extensions;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;

namespace MIS.Demo.Repositories
{
	public class DispanserizationsRepository : IDispanserizationsRepository
	{
		private readonly DemoDataContext _dataContext;
		private readonly IDateTimeProvider _dateTimeProvider;

		public DispanserizationsRepository(
			IDateTimeProvider dateTimeProvider,
			DemoDataContext dataContext
		)
		{
			_dateTimeProvider = dateTimeProvider;
			_dataContext = dataContext;
		}

		public int Create(Dispanserization dispanserization)
		{
			if (_dataContext.Dispanserizations.FirstOrDefault(
					d => d.PatientID == dispanserization.PatientID
					&& dispanserization.BeginDate.Year == _dateTimeProvider.Now.Year
				) != null
			)
			{
				throw new Exception("Dispanserization already exists!");
			}

			var patient = _dataContext.Patients
				.FirstOrDefault(p => p.ID == dispanserization.PatientID);

			var resources = _dataContext.Resources
				.Where(r => r.Employee.Specialty.ID == 0)
				.ToList();

			dispanserization.Researches = new List<Research>();
			foreach (var resource in resources)
			{
				dispanserization.Researches.Add(new Research
				{
					ID = resource.ID * 10 + dispanserization.ID,
					Description = $"{resource.Employee.GetName()} в {resource.Room.Code} каб."
				});

				var timeItem = _dataContext.TimeItems
					.OrderBy(ti => ti.ResourceID)
					.ThenBy(ti => ti.BeginDateTime)
					.FirstOrDefault(ti => ti.ResourceID == resource.ID && ti.VisitItem == null);

				var visitItem = new VisitItem
				{
					ID = _dataContext.VisitItems.Max(vi => vi.ID) + 1,
					TimeItem = timeItem,
					TimeItemID = timeItem.ID,
					Patient = patient,
					PatientID = patient.ID
				};

				visitItem.TimeItem.VisitItem = visitItem;

				_dataContext.VisitItems.Add(visitItem);
			}

			dispanserization.ID = _dataContext.Dispanserizations.Count > 0 ? _dataContext.Dispanserizations.Max(d => d.ID) + 1 : 1;

			_dataContext.Dispanserizations.Add(dispanserization);

			var result = dispanserization.ID;

			return result;
		}

		public Dispanserization Get(int dispanserizationID)
		{
			var result = _dataContext.Dispanserizations
				.FirstOrDefault(d => d.ID == dispanserizationID);

			return result;
		}

		public List<Dispanserization> ToList(int patientID)
		{
			var result = _dataContext.Dispanserizations
				.Where(d => d.PatientID == patientID)
				.ToList();

			return result;
		}
	}
}
