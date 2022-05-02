using MIS.Domain.Entities;
using System.Text;

namespace MIS.Domain.Extensions
{
	public static class EmployeeExtension
	{
		public static string GetName(this Employee employee)
		{
			var sb = new StringBuilder();

			if (!string.IsNullOrEmpty(employee.LastName))
			{
				sb.Append(employee.LastName);
			}
			if (!string.IsNullOrEmpty(employee.FirstName))
			{
				sb.Append($" {employee.FirstName[0]}.");
			}
			if (!string.IsNullOrEmpty(employee.MiddleName))
			{
				sb.Append($" {employee.MiddleName[0]}.");
			}

			return sb.ToString();
		}

	}
}
