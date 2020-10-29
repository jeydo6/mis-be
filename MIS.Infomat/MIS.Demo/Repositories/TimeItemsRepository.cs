using MIS.Demo.DataContexts;
using MIS.Domain.Entities;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MIS.Demo.Repositories
{
    public class TimeItemsRepository : ITimeItemsRepository
    {
        private readonly DemoDataContext _dataContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public TimeItemsRepository(
            IDateTimeProvider dateTimeProvider,
            DemoDataContext dataContext
        )
        {
            _dateTimeProvider = dateTimeProvider;
            _dataContext = dataContext;
        }

        public IEnumerable<TimeItem> ToList(DateTime beginDate, DateTime endDate, Int32 resourceID = 0)
        {
            return _dataContext.TimeItems
                .Where(ti => ti.Resource.Doctor.Specialty.ID > 0)
                .Where(ti => ti.Date >= beginDate && ti.Date <= endDate && (resourceID == 0 || ti.ResourceID == resourceID))
                .ToList();
        }

        public IEnumerable<TimeItemTotal> GetResourceTotals(DateTime beginDate, DateTime endDate, Int32 specialtyID = 0)
        {
            return _dataContext.TimeItems
                .Where(ti => ti.Date >= beginDate && ti.Date <= endDate && (specialtyID == 0 || ti.Resource.Doctor.SpecialtyID == specialtyID))
                .Where(ti => ti.Resource.Doctor.Specialty.ID > 0)
                .GroupBy(ti => new { ti.ResourceID, ti.Date })
                .Select(g => new TimeItemTotal
                {
                    ResourceID = g.Key.ResourceID,
                    Date = g.Key.Date,
                    TimesCount = g.Count(),
                    VisitsCount = g.Count(ti => ti.VisitItem != null)
                });
        }

        public IEnumerable<TimeItemTotal> GetDispanserizationTotals(DateTime beginDate, DateTime endDate)
        {
            return _dataContext.TimeItems
                .Where(ti => ti.Date >= beginDate && ti.Date <= endDate)
                .Where(ti => ti.Resource.Doctor.Specialty.ID == 0)
                .GroupBy(ti => new { ti.ResourceID, ti.Date })
                .Select(g => new TimeItemTotal
                {
                    ResourceID = g.Key.ResourceID,
                    Date = g.Key.Date,
                    TimesCount = g.Count(),
                    VisitsCount = g.Count(ti => ti.VisitItem != null)
                });
        }
    }
}
