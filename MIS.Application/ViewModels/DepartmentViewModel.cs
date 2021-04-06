using System;

namespace MIS.Application.ViewModels
{
	public class DepartmentViewModel
	{
		public String DepartmentName { get; set; }

		public EmployeeViewModel[] Employees { get; set; }
	}
}
