using MIS.Application.Interfaces;
using System;

namespace MIS.Application.ViewModels
{
	public class DepartmentViewModel : ISeparable<DepartmentViewModel>
	{
		public String DepartmentName { get; set; }

		public EmployeeViewModel[] Employees { get; set; }

		public (DepartmentViewModel current, DepartmentViewModel next) Separate(ref Int32 length)
		{
			if (Employees.Length > length)
			{
				DepartmentViewModel current = new DepartmentViewModel
				{
					DepartmentName = DepartmentName,
					Employees = Employees[..length]
				};

				DepartmentViewModel next = new DepartmentViewModel
				{
					DepartmentName = DepartmentName,
					Employees = Employees[length..]
				};

				length = current.Employees.Length;
				return (current, next);
			}

			length = Employees.Length;
			return (this, null);
		}
	}
}
