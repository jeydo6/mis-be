using MIS.Demo.DataContexts;
using MIS.Domain.Entities;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public Int32 Create(Dispanserization dispanserization)
        {
            if (_dataContext.Dispanserizations.FirstOrDefault(
                    d => d.PatientID == dispanserization.PatientID
                    && dispanserization.BeginDate.Year == _dateTimeProvider.Now.Year
                ) != null
            )
            {
                throw new Exception("Dispanserization already exists!");
            }

            Patient patient = _dataContext.Patients
                .FirstOrDefault(p => p.ID == dispanserization.PatientID);

            IEnumerable<Resource> resources = _dataContext.Resources
                .Where(r => r.Doctor.Specialty.ID == 0)
                .ToList();

            dispanserization.Analyses = new List<Analysis>();
            foreach (Resource resource in resources)
            {
                dispanserization.Analyses.Add(new Analysis
                {
                    ID = resource.ID * 10 + dispanserization.ID,
                    Description = $"{resource.Doctor.DisplayName} в {resource.Room.Code} каб."
                });

                TimeItem timeItem = _dataContext.TimeItems
                    .OrderBy(ti => ti.ResourceID)
                    .ThenBy(ti => ti.BeginDateTime)
                    .FirstOrDefault(ti => ti.ResourceID == resource.ID && ti.VisitItem == null);

                VisitItem visitItem = new VisitItem
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

            return dispanserization.ID;
        }

        public Dispanserization Get(Int32 dispanserizationID)
        {
            return _dataContext.Dispanserizations
                .FirstOrDefault(d => d.ID == dispanserizationID);
        }

        public IEnumerable<Dispanserization> ToList(Int32 patientID)
        {
            return _dataContext.Dispanserizations
                .Where(d => d.PatientID == patientID)
                .ToList();
        }
    }
}
