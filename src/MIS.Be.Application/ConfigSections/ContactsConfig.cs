using System;

namespace MIS.Be.Application.Configs
{
	public class ContactsConfig
	{
		public class Department
		{
			public string DepartmentName { get; set; }

			public Employee[] Employees { get; set; }
		}

		public class Employee
		{
			public string EmployeeName { get; set; }

			public string PostName { get; set; }

			public DateTime BeginTime { get; set; }

			public DateTime EndTime { get; set; }

			public string PhoneNumber { get; set; }

			public string RoomCode { get; set; }
		}

		public Department[] Departments { get; set; }
	}
}
