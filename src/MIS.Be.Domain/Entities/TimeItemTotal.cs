using System;

namespace MIS.Be.Domain.Entities
{
	public class TimeItemTotal
	{
		public int ResourceID { get; set; }

		public DateTime Date { get; set; }

		public DateTime BeginDateTime { get; set; }

		public DateTime EndDateTime { get; set; }

		public int TimesCount { get; set; }

		public int VisitsCount { get; set; }
	}
}
