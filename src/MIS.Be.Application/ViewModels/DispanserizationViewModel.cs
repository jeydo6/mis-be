using System;

namespace MIS.Be.Application.ViewModels
{
	public class DispanserizationViewModel
	{
		public DateTime BeginDate { get; set; }

		public string PatientCode { get; set; }

		public string PatientName { get; set; }

		public bool IsClosed { get; set; }

		public bool IsEnabled { get; set; }

		public string[] Researches { get; set; }
	}
}
