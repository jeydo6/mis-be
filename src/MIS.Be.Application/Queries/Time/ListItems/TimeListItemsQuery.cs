using System;
using MIS.Be.Application.ViewModels;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Queries
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
