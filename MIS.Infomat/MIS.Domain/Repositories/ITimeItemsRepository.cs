using MIS.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MIS.Domain.Repositories
{
    public interface ITimeItemsRepository
    {
        IEnumerable<TimeItem> ToList(DateTime beginDate, DateTime endDate, Int32 resourceID = 0);

        IEnumerable<TimeItemTotal> GetResourceTotals(DateTime beginDate, DateTime endDate, Int32 specialtyID = 0);

        IEnumerable<TimeItemTotal> GetDispanserizationTotals(DateTime beginDate, DateTime endDate);
    }
}
