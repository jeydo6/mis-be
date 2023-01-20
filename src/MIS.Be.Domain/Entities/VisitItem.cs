
namespace MIS.Be.Domain.Entities
{
	public class VisitItem
	{
		public int ID { get; set; }

		public int PatientID { get; set; }

		public Patient Patient { get; set; }

		public int TimeItemID { get; set; }

		public TimeItem TimeItem { get; set; }
	}
}
