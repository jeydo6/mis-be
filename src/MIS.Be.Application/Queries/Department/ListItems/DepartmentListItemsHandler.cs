using System;
using System.Linq;
using Microsoft.Extensions.Options;
using MIS.Be.Application.Configs;
using MIS.Be.Application.ViewModels;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Queries
{
	public class DepartmentListItemsHandler : IRequestHandler<DepartmentListItemsQuery, DepartmentViewModel[]>
	{
		private readonly ContactsConfig _contactsConfig;

		public DepartmentListItemsHandler(
			IOptionsMonitor<ContactsConfig> contactsConfigOptions
		)
		{
			_contactsConfig = contactsConfigOptions.CurrentValue;
		}

		public DepartmentViewModel[] Handle(DepartmentListItemsQuery request)
		{
			var result = _contactsConfig.Departments != null
				? _contactsConfig.Departments
					.Select(d => new DepartmentViewModel
					{
						DepartmentName = d.DepartmentName,
						Employees = d.Employees
							.Select(e => new EmployeeViewModel
							{
								EmployeeName = e.EmployeeName,
								BeginDateTime = e.BeginTime,
								EndDateTime = e.EndTime,
								PhoneNumber = e.PhoneNumber,
								PostName = e.PostName,
								RoomCode = e.RoomCode
							})
							.ToArray()
					})
					.ToArray()
				: Array.Empty<DepartmentViewModel>();

			return result;
		}
	}
}
