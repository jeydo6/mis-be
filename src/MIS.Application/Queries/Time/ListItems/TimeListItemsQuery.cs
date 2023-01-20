using System;
using MIS.Application.ViewModels;
using MIS.Mediator;

namespace MIS.Application.Queries
{
	public class TimeListItemsQuery : IRequest<TimeItemViewModel[]>
	{
		public TimeListItemsQuery(DateTime date, int resourceID)
		{
			Date = date;
			ResourceID = resourceID;
		}

		public DateTime Date { get; set; }

		public int ResourceID { get; set; }
	}
}
