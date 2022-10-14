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

using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories
{
	public class DispanserizationsRepository : BaseRepository, IDispanserizationsRepository
	{
		public DispanserizationsRepository(string connectionString) : base(connectionString) { }

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

		public Dispanserization Get(int dispanserizationID)
		{
			using (var db = OpenConnection())
			{
				var dispanserizations = new Dictionary<int, Dispanserization>();

				return db.Query<Dispanserization, Research, Dispanserization>(
					sql: "[dbo].[sp_Dispanserizations_Get]",
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
					param: new { dispanserizationID },
					commandType: CommandType.StoredProcedure
				).Distinct().FirstOrDefault();
			}
		}

		public List<Dispanserization> ToList(int patientID)
		{
			using (var db = OpenConnection())
			{
				var dispanserizations = new Dictionary<int, Dispanserization>();

				return db.Query<Dispanserization, Research, Dispanserization>(
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
				).Distinct().AsList();
			}
		}
	}
}