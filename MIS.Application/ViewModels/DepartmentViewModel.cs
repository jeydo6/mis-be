using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MIS.Application.ViewModels
{
	public class DepartmentViewModel : IGrouping<DepartmentViewModel, EmployeeViewModel>
	{
		public string DepartmentName { get; set; }

		public EmployeeViewModel[] Employees { get; set; }

		public DepartmentViewModel Key => this;

		public IEnumerator<EmployeeViewModel> GetEnumerator()
		{
			foreach (var employee in Employees)
			{
				yield return employee;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
