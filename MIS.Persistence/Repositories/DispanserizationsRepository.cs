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
using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories
{
	public class DispanserizationsRepository : RepositoryBase, IDispanserizationsRepository
	{
		public DispanserizationsRepository(IConfiguration configuration) : base(configuration) { }

		public int Create(Dispanserization item)
		{
			using (var db = OpenConnection())
			using (var transaction = db.BeginTransaction(IsolationLevel.ReadUncommitted))
			{
				try
				{
					var id = db.QuerySingle<int>(
						sql: "[dbo].[sp_Dispanserizations_Create]",
						param: new
						{
							patientID = item.PatientID,
							beginDate = item.BeginDate,
							endDate = item.EndDate
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

		public Dispanserization Get(int id)
		{
			using var db = OpenConnection();

			var dispanserizations = new Dictionary<int, Dispanserization>();

			var items = db.Query<Dispanserization, Research, Dispanserization>(
				sql: "[dbo].[sp_Dispanserizations_Get]",
				map: (dispanserization, research) =>
				{
					if (!dispanserizations.ContainsKey(dispanserization.ID))
					{
						dispanserizations[dispanserization.ID] = dispanserization;
					}

					var result = dispanserizations[dispanserization.ID];
					result.Researches.Add(research);

					return result;
				},
				param: new { id },
				commandType: CommandType.StoredProcedure
			).AsList();

			if (!dispanserizations.ContainsKey(id))
			{
				throw new Exception($"Диспансеризация с id = {id} не найдена");
			}

			return dispanserizations[id];
		}

		public List<Dispanserization> ToList(int patientID)
		{
			using var db = OpenConnection();

			var dispanserizations = new Dictionary<int, Dispanserization>();

			var items = db.Query<Dispanserization, Research, Dispanserization>(
					sql: "[dbo].[sp_Dispanserizations_List]",
					map: (dispanserization, research) =>
					{
						if (!dispanserizations.TryGetValue(dispanserization.ID, out var value))
						{
							value = dispanserization;
							value.Researches = new List<Research>();
							dispanserizations[dispanserization.ID] = dispanserization;
						}

						value.Researches.Add(research);
						return value;
					},
					param: new { patientID },
					commandType: CommandType.StoredProcedure
				).AsList();

			return new List<Dispanserization>(dispanserizations.Values);
		}
	}
}