using MIS.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MIS.Domain.Repositories
{
    public interface IVisitItemsRepository
    {
        Int32 Create(VisitItem visitItem);

        VisitItem Get(Int32 visitItemID);

        IEnumerable<VisitItem> ToList(DateTime beginDate, DateTime endDate, Int32 patientID = 0);
    }
}
