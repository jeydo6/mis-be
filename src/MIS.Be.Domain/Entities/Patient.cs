using System;
using System.Collections.Generic;
using MIS.Be.Domain.Enums;

namespace MIS.Be.Domain.Entities
{
	public class Patient
	{
		public Patient()
		{
			VisitItems = new List<VisitItem>();
			Dispanserizations = new List<Dispanserization>();
		}

		public int ID { get; set; }

		public string Code { get; set; }

		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public DateTime BirthDate { get; set; }

		public Gender Gender { get; set; }

		public ICollection<VisitItem> VisitItems { get; set; }

		public ICollection<Dispanserization> Dispanserizations { get; set; }
	}
}
