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
using System.Data;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories
{
	public class PatientsRepository : BaseRepository, IPatientsRepository
	{
		public PatientsRepository(string connectionString) : base(connectionString) { }

		public int Create(Patient item)
		{
			using (var db = OpenConnection())
			using (var transaction = db.BeginTransaction(IsolationLevel.ReadUncommitted))
			{
				try
				{
					var id = db.QuerySingle<int>(
						sql: "[dbo].[sp_Patients_Create]",
						param: new
						{
							code = item.Code,
							firstName = item.FirstName,
							middleName = item.MiddleName,
							lastName = item.LastName,
							birthDate = item.BirthDate,
							gender = item.Gender,
						},
						commandType: CommandType.StoredProcedure,
						transaction: transaction
					);

					transaction.Commit();
					return id;
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public Patient First(string code, DateTime birthDate)
		{
			using (var db = OpenConnection())
			{
				return db.QueryFirstOrDefault<Patient>(
					sql: "[dbo].[sp_Patients_First]",
					param: new { code, birthDate },
					commandType: CommandType.StoredProcedure
				);
			}
		}

		public Patient Get(int id)
		{
			using var db = OpenConnection();

			var item = db.QueryFirstOrDefault<Patient>(
				sql: "[dbo].[sp_Patients_Get]",
				param: new { id },
				commandType: CommandType.StoredProcedure
			);

			if (item == null)
			{
				throw new Exception($"Пациент с id = {id} не найден");
			}

			return item;
		}
	}
}