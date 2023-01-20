namespace MIS.Application.ViewModels
{
	public class ResourceViewModel
	{
		public string EmployeeName { get; set; }

		public string RoomCode { get; set; }

		public int Count { get; set; }

		public bool IsEnabled { get; set; }

		public bool IsBlocked { get; set; }

		public int ResourceID { get; set; }

		public int SpecialtyID { get; set; }

		public DateItemViewModel[] Dates { get; set; }
	}
}
