namespace MIS.Be.Domain.Entities
{
	public class Employee
	{
		public int ID { get; set; }

		public string Code { get; set; }

		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public int SpecialtyID { get; set; }

		public Specialty Specialty { get; set; }
	}
}
