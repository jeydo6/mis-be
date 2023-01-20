using System;

namespace MIS.Application.ViewModels
{
	public class VisitItemViewModel
	{
		public DateTime BeginDateTime { get; set; }

		public string PatientCode { get; set; }

		public string PatientName { get; set; }

		public string ResourceName { get; set; }

		public string EmployeeName { get; set; }

		public string SpecialtyName { get; set; }

		public string RoomCode { get; set; }

		public int RoomFloor { get; set; }

		public bool IsEnabled { get; set; }

		public int ResourceID { get; set; }
	}
}
