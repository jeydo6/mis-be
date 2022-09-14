#region Copyright © 2018-2022 Vladimir Deryagin. All rights reserved
/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using MIS.Application.Configs;
using MIS.Application.ViewModels;

namespace MIS.Application.Queries
{
	public class DepartmentListItemsHandler : IRequestHandler<DepartmentListItemsQuery, DepartmentViewModel[]>
	{
		private readonly ContactsConfigSection _contactsConfig;

		public DepartmentListItemsHandler(
			IOptionsMonitor<ContactsConfigSection> contactsConfigOptions
		)
		{
			_contactsConfig = contactsConfigOptions.CurrentValue;
		}

		public async Task<DepartmentViewModel[]> Handle(DepartmentListItemsQuery request, CancellationToken cancellationToken)
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
							BeginTime = e.BeginTime,
							EndTime	= e.EndTime,
							PhoneNumber = e.PhoneNumber,
							PostName = e.PostName,
							RoomCode = e.RoomCode
						})
							.ToArray()
					})
					.ToArray()
				: Array.Empty<DepartmentViewModel>();

			return await Task.FromResult(result);
		}
	}
}
