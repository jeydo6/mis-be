using MediatR;
using Microsoft.Extensions.Options;
using MIS.Application.Configs;
using MIS.Application.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Queries
{
	public class DepartmentListItemsHandler : IRequestHandler<DepartmentListItemsQuery, DepartmentViewModel[]>
	{
		private readonly ContactsConfig _contactsConfig;

		public DepartmentListItemsHandler(
			IOptionsSnapshot<ContactsConfig> contactsConfigOptions
		)
		{
			_contactsConfig = contactsConfigOptions.Value;
		}

		public async Task<DepartmentViewModel[]> Handle(DepartmentListItemsQuery request, CancellationToken cancellationToken)
		{
			var result = _contactsConfig.Departments != null
				? _contactsConfig.Departments.ToArray()
				: Array.Empty<DepartmentViewModel>();

			return await Task.FromResult(result);
		}
	}
}
