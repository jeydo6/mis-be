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
using Dapper;
using Microsoft.Extensions.Configuration;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories
{
	public class VisitItemsRepository : RepositoryBase, IVisitItemsRepository
	{
		public VisitItemsRepository(IConfiguration configuration) : base(configuration) { }

		public int Create(VisitItem item)
		{
			using (var db = OpenConnection())
			using (var transaction = db.BeginTransaction(IsolationLevel.ReadUncommitted))
			{
				try
				{
					var id = db.QuerySingle<int>(
						sql: "[dbo].[sp_VisitItems_Create]",
						param: new
						{
							patientID = item.PatientID,
							timeItemID = item.TimeItemID
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

		public VisitItem Get(int visitItemID)
		{
			using (var db = OpenConnection())
			{
				return db.Query<VisitItem, TimeItem, Resource, Employee, Specialty, Room, VisitItem>(
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
					commandType: CommandType.StoredProcedure
				).FirstOrDefault();
			}
		}

		public List<VisitItem> ToList(DateTime beginDate, DateTime endDate, int patientID = 0)
		{
			using (var db = OpenConnection())
			{
				return db.Query<VisitItem, TimeItem, Resource, Employee, Specialty, Room, VisitItem>(
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
					commandType: CommandType.StoredProcedure
				).AsList();
			}
		}
	}
}
