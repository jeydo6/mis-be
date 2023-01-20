using System;
using System.Linq;
using MIS.Be.Demo.DataContexts;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Demo.Repositories
{
	public class PatientsRepository : IPatientsRepository
	{
		private readonly DemoDataContext _dataContext;

		public PatientsRepository(
			DemoDataContext dataContext
		)
		{
			_dataContext = dataContext;
		}

		public int Create(Patient item)
		{
			item.ID = _dataContext.Patients.LastOrDefault()?.ID ?? 1;
			_dataContext.Patients.Add(item);

			return item.ID;
		}

		public Patient Find(string code, DateTime birthDate)
		{
			var result = _dataContext.Patients
				.FirstOrDefault(s => s.Code == code && s.BirthDate == birthDate);

			return result;
		}

		public Patient Get(int patientID)
		{
			var result = _dataContext.Patients
				.FirstOrDefault(s => s.ID == patientID);

			return result;
		}

	}
}
