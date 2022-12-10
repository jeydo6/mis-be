using System;

namespace MIS.Domain.Entities
{
	public class TimeItem
	{
		public int ID { get; set; }

		public DateTime Date { get; set; }

		public DateTime BeginDateTime { get; set; }

		public DateTime EndDateTime { get; set; }

		public int ResourceID { get; set; }

		public Resource Resource { get; set; }

		public VisitItem VisitItem { get; set; }
	}
}
