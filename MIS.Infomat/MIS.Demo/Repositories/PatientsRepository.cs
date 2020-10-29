using MIS.Demo.DataContexts;
using MIS.Domain.Entities;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;
using System;
using System.Linq;

namespace MIS.Demo.Repositories
{
    public class PatientsRepository : IPatientsRepository
    {
        private readonly DemoDataContext _dataContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public PatientsRepository(
            IDateTimeProvider dateTimeProvider,
            DemoDataContext dataContext
        )
        {
            _dateTimeProvider = dateTimeProvider;
            _dataContext = dataContext;
        }

        public Patient First(String code, DateTime birthDate)
        {
            return _dataContext.Patients
                .FirstOrDefault(s => s.Code == code && s.BirthDate == birthDate);
        }

        public Patient Get(Int32 patientID)
        {
            return _dataContext.Patients
                .FirstOrDefault(s => s.ID == patientID);
        }

    }
}