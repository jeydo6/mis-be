#region Copyright © 2020 Vladimir Deryagin. All rights reserved
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

using MIS.Demo.DataContexts;
using MIS.Domain.Entities;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MIS.Demo.Repositories
{
	public class PatientsRepository : IPatientsRepository
	{
		private readonly DemoDataContext _dataContext;

		public PatientsRepository(
			IDateTimeProvider _,
			DemoDataContext dataContext
		)
		{
			_dataContext = dataContext;
		}

		public async Task<Patient> First(String code, DateTime birthDate)
		{
			var result = _dataContext.Patients
				.FirstOrDefault(s => s.Code == code && s.BirthDate == birthDate);

			return await Task.FromResult(result);
		}

		public async Task<Patient> Get(Int32 patientID)
		{
			var result = _dataContext.Patients
				.FirstOrDefault(s => s.ID == patientID);

			return await Task.FromResult(result);
		}

	}
}