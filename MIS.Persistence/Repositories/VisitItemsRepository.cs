#region Copyright © 2020-2021 Vladimir Deryagin. All rights reserved
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
	public class VisitItemsRepository : IVisitItemsRepository, IDisposable
	{
		private readonly IDbConnection _db;
		private readonly IDbTransaction _transaction;

		public VisitItemsRepository(String connectionString)
		{
			_db = new SqlConnection(connectionString);
			_transaction = null;
		}

		public VisitItemsRepository(IDbTransaction transaction)
		{
			_db = transaction.Connection;
			_transaction = transaction;
		}

		public async Task<Int32> Create(VisitItem item)
		{
			var result = await _db.QuerySingleAsync<Int32>(
				sql: "[dbo].[sp_VisitItems_Create]",
				param: new
				{
					patientID = item.PatientID,
					timeItemID = item.TimeItemID
				},
				commandType: CommandType.StoredProcedure,
				transaction: _transaction
			);

			return result;
		}

		public async Task<VisitItem> Get(Int32 visitItemID)
		{
			var result = await _db.QueryAsync<VisitItem, TimeItem, Resource, Employee, Specialty, Room, VisitItem>(
				sql: "[dbo].[sp_VisitItems_Get]",
				map: (visitItem, timeItem, resource, employee, specialty, room) =>
				{
					visitItem.TimeItem = timeItem;
					visitItem.TimeItem.Resource = resource;
					visitItem.TimeItem.Resource.Employee = employee;
					visitItem.TimeItem.Resource.Employee.Specialty = specialty;
					visitItem.TimeItem.Resource.Room = room;
					visitItem.TimeItem.VisitItem = visitItem;

					return visitItem;
				},
				param: new { visitItemID },
				commandType: CommandType.StoredProcedure,
				transaction: _transaction
			);

			return result
				.FirstOrDefault();
		}

		public async Task<List<VisitItem>> ToList(DateTime beginDate, DateTime endDate, Int32 patientID = 0)
		{
			var result = await _db.QueryAsync<VisitItem, TimeItem, Resource, Employee, Specialty, Room, VisitItem>(
				sql: "[dbo].[sp_VisitItems_List]",
				map: (visitItem, timeItem, resource, employee, specialty, room) =>
				{
					visitItem.TimeItem = timeItem;
					visitItem.TimeItem.Resource = resource;
					visitItem.TimeItem.Resource.Employee = employee;
					visitItem.TimeItem.Resource.Employee.Specialty = specialty;
					visitItem.TimeItem.Resource.Room = room;
					visitItem.TimeItem.VisitItem = visitItem;

					return visitItem;
				},
				param: new { beginDate, endDate, patientID },
				commandType: CommandType.StoredProcedure,
				transaction: _transaction
			);

			return result
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
