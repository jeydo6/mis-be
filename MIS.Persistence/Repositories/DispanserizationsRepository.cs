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
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories
{
	public class DispanserizationsRepository : IDispanserizationsRepository, IDisposable
	{
		private readonly IDbConnection _db;
		private readonly IDbTransaction _transaction;

		public DispanserizationsRepository(string connectionString)
		{
			_db = new SqlConnection(connectionString);
			_transaction = null;
		}

		public DispanserizationsRepository(IDbTransaction transaction)
		{
			_db = transaction.Connection;
			_transaction = transaction;
		}

		public async Task<int> Create(Dispanserization item)
		{
			int dispanserizationID = await _db.QuerySingleAsync<int>(
				sql: "[dbo].[sp_Dispanserizations_Create]",
				param: new
				{
					patientID = item.PatientID,
					beginDate = item.BeginDate,
					endDate = item.EndDate
				},
				commandType: CommandType.StoredProcedure,
				transaction: _transaction
			);

			return dispanserizationID;
		}

		public async Task<Dispanserization> Get(int dispanserizationID)
		{
			var dispanserizations = new Dictionary<int, Dispanserization>();

			var query = await _db.QueryAsync<Dispanserization, Research, Dispanserization>(
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
				commandType: CommandType.StoredProcedure,
				transaction: _transaction
			);

			return query
				.Distinct()
				.FirstOrDefault();
		}

		public async Task<List<Dispanserization>> ToList(int patientID)
		{
			var dispanserizations = new Dictionary<int, Dispanserization>();

			var query = await _db.QueryAsync<Dispanserization, Research, Dispanserization>(
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
				commandType: CommandType.StoredProcedure,
				transaction: _transaction
			);

			return query
				.Distinct()
				.AsList();
		}

		public void Dispose()
		{
			if (_db != null)
			{
				_db.Dispose();
				GC.SuppressFinalize(this);
			}
		}
	}
}