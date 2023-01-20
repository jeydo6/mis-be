using System;
using System.Collections.Generic;

namespace MIS.Be.Application.ViewModels
{
	public class PatientViewModel
	{
		public PatientViewModel()
		{
			VisitItems = new List<VisitItemViewModel>();
			Dispanserizations = new List<DispanserizationViewModel>();
		}

		public int ID { get; set; }

		public string Code { get; set; }

		public string Name { get; set; }

		public DateTime BirthDate { get; set; }

		public List<VisitItemViewModel> VisitItems { get; set; }

		public List<DispanserizationViewModel> Dispanserizations { get; set; }
	}
}
