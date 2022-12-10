using System.Linq;
using MIS.Application.ViewModels;
using MIS.Domain.Extensions;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;
using MIS.Mediator;

namespace MIS.Application.Queries
{
	public class PatientFirstHandler : IRequestHandler<PatientFirstQuery, PatientViewModel>
	{
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly IPatientsRepository _patients;
		private readonly IVisitItemsRepository _visitItems;
		private readonly IDispanserizationsRepository _dispanserizations;

		public PatientFirstHandler(
			IDateTimeProvider dateTimeProvider,
			IPatientsRepository patients,
			IVisitItemsRepository visitItems,
			IDispanserizationsRepository dispanserizations
		)
		{
			_dateTimeProvider = dateTimeProvider;
			_patients = patients;
			_visitItems = visitItems;
			_dispanserizations = dispanserizations;
		}

		public PatientViewModel Handle(PatientFirstQuery request)
		{
			var patient = _patients.Find(request.Code, request.BirthDate);

			if (patient == null)
			{
				return null;
			}

			var beginDate = _dateTimeProvider.Now.Date;
			var endDate = _dateTimeProvider.Now.Date.AddDays(28);

			patient.VisitItems = _visitItems
				.ToList(beginDate, endDate, patientID: patient.ID);

			patient.Dispanserizations = _dispanserizations
				.ToList(patient.ID);

			var patientName = patient.GetName();

			var result = new PatientViewModel
			{
				ID = patient.ID,
				Code = patient.Code,
				Name = patientName,
				BirthDate = patient.BirthDate,
				Dispanserizations = patient.Dispanserizations.Select(d => new DispanserizationViewModel
				{
					BeginDate = d.BeginDate,
					PatientCode = patient.Code,
					PatientName = patientName,
					IsClosed = d.IsClosed,
					IsEnabled = true,
					Researches = d.Researches.Select(a => a.Description).ToArray()
				}).ToList(),
				VisitItems = patient.VisitItems.Select(vi => new VisitItemViewModel
				{
					BeginDateTime = vi.TimeItem.BeginDateTime,
					PatientCode = patient.Code,
					PatientName = patientName,
					ResourceName = vi.TimeItem.Resource.Name,
					EmployeeName = vi.TimeItem.Resource.Employee.GetName(),
					SpecialtyName = vi.TimeItem.Resource.Employee.Specialty.Name,
					RoomCode = vi.TimeItem.Resource.Room.Code,
					RoomFloor = vi.TimeItem.Resource.Room.Floor,
					IsEnabled = true,
					ResourceID = vi.TimeItem.ResourceID
				}).ToList()
			};

			return result;
		}
	}
}
