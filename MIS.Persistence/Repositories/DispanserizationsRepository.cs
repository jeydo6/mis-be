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

using Dapper;
using Microsoft.Data.SqlClient;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MIS.Persistence.Repositories
{
	public class DispanserizationsRepository : IDispanserizationsRepository, IDisposable
	{
		private readonly IDbConnection _db;
		private readonly IDbTransaction _transaction;

		public DispanserizationsRepository(String connectionString)
		{
			_db = new SqlConnection(connectionString);
			_transaction = null;
		}

		public DispanserizationsRepository(IDbTransaction transaction)
		{
			_db = transaction.Connection;
			_transaction = transaction;
		}

		public async Task<Int32> Create(Dispanserization item)
		{
			Int32 dispanserizationID = await _db.QuerySingleAsync<Int32>(
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

		public async Task<Dispanserization> Get(Int32 dispanserizationID)
		{
			var keyValues = new Dictionary<Int32, Dispanserization>();

			var result = await _db.QueryAsync<Dispanserization, Analysis, Dispanserization>(
				sql: "[dbo].[sp_Dispanserizations_Get]",
				map: (dispanserization, analysis) =>
				{
					if (!keyValues.TryGetValue(dispanserization.ID, out Dispanserization value))
					{
						value = dispanserization;
						value.Analyses = new List<Analysis>();
						keyValues.Add(dispanserization.ID, dispanserization);
					}

					value.Analyses.Add(analysis);

					return dispanserization;
				},
				param: new { dispanserizationID },
				commandType: CommandType.StoredProcedure,
				transaction: _transaction
			);

			return result
				.Distinct()
				.FirstOrDefault();
		}

		public async Task<List<Dispanserization>> ToList(Int32 patientID)
		{
			var keyValues = new Dictionary<Int32, Dispanserization>();

			var result = await _db.QueryAsync<Dispanserization, Analysis, Dispanserization>(
				sql: "[dbo].[sp_Dispanserizations_List]",
				map: (dispanserization, analysis) =>
				{
					if (!keyValues.TryGetValue(dispanserization.ID, out Dispanserization value))
					{
						value = dispanserization;
						value.Analyses = new List<Analysis>();
						keyValues.Add(dispanserization.ID, dispanserization);
					}

					value.Analyses.Add(analysis);
					return value;
				},
				param: new { patientID },
				commandType: CommandType.StoredProcedure,
				transaction: _transaction
			);

			return result
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