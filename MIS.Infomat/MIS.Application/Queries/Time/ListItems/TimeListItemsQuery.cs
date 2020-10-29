using MediatR;
using MIS.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace MIS.Application.Queries
{
    public class TimeListItemsQuery : IRequest<IEnumerable<TimeItemViewModel>>
    {
        public TimeListItemsQuery(DateTime date, Int32 resourceID)
        {
            Date = date;
            ResourceID = resourceID;
        }

        public DateTime Date { get; set; }

        public Int32 ResourceID { get; set; }
    }
}
