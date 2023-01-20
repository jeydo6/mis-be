using System;

namespace MIS.Application.ViewModels
{
	public class DateItemViewModel
	{
		public DateTime Date { get; set; }

		public DateTime BeginDateTime { get; set; }

		public DateTime EndDateTime { get; set; }

		public int Count { get; set; }

		public bool IsEnabled { get; set; }

		public bool IsBlocked { get; set; }

		public int ResourceID { get; set; }

		public TimeItemViewModel[] Times { get; set; }
	}
}
