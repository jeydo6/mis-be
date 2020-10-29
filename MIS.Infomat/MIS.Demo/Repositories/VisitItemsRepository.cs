using MIS.Demo.DataContexts;
using MIS.Domain.Entities;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MIS.Demo.Repositories
{
    public class VisitItemsRepository : IVisitItemsRepository
    {
        private readonly DemoDataContext _dataContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public VisitItemsRepository(
            IDateTimeProvider dateTimeProvider,
            DemoDataContext dataContext
        )
        {
            _dateTimeProvider = dateTimeProvider;
            _dataContext = dataContext;
        }

        public Int32 Create(VisitItem visitItem)
        {
            if (_dataContext.VisitItems.FirstOrDefault(vi => vi.TimeItemID == visitItem.TimeItemID) != null)
            {
                throw new Exception("Visit item already exists!");
            }

            visitItem.TimeItem = _dataContext.TimeItems.FirstOrDefault(ti => ti.ID == visitItem.TimeItemID);
            visitItem.TimeItem.VisitItem = visitItem;

            visitItem.Patient = _dataContext.Patients.FirstOrDefault(p => p.ID == visitItem.PatientID);

            visitItem.ID = _dataContext.VisitItems.Count > 0 ? _dataContext.VisitItems.Max(vi => vi.ID) + 1 : 1;

            _dataContext.VisitItems.Add(visitItem);

            return visitItem.ID;
        }

        public VisitItem Get(Int32 visitItemID)
        {
            return _dataContext.VisitItems
                .FirstOrDefault(vi => vi.ID == visitItemID);
        }

        public IEnumerable<VisitItem> ToList(DateTime beginDate, DateTime endDate, Int32 patientID = 0)
        {
            return _dataContext.VisitItems
                .Where(vi => vi.TimeItem.Resource.Doctor.Specialty.ID > 0)
                .Where(vi => vi.TimeItem.Date >= beginDate && vi.TimeItem.Date <= endDate && (patientID == 0 || vi.PatientID == patientID))
                .ToList();
        }
    }
}
