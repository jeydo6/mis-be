using System;
using System.Collections.Generic;

namespace MIS.Be.Domain.Entities
{
	public class Dispanserization
	{
		public int ID { get; set; }

		public DateTime BeginDate { get; set; }

		public DateTime EndDate { get; set; }

		public bool IsClosed { get; set; }

		public int PatientID { get; set; }

		public ICollection<Research> Researches { get; set; } = new List<Research>();
	}
}
