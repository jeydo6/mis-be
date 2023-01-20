using MIS.Be.Domain.Enums;

namespace MIS.Be.Domain.Entities
{
	public class Resource
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public ResourceType Type { get; set; }

		public int EmployeeID { get; set; }

		public Employee Employee { get; set; }

		public int RoomID { get; set; }

		public Room Room { get; set; }
	}
}
