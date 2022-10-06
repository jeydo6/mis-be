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
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories
{
	public class TimeItemsRepository : BaseRepository, ITimeItemsRepository
	{
		public TimeItemsRepository(string connectionString) : base(connectionString) { }

		public List<TimeItem> ToList(DateTime beginDate, DateTime endDate, int resourceID = 0)
		{
			using (var db = OpenConnection())
			{
				return db.Query<TimeItem, Resource, Employee, Specialty, Room, VisitItem, TimeItem>(
					sql: "[dbo].[sp_TimeItems_List]",
					map: (timeItem, resource, employee, specialty, room, visitItem) =>
					{
						timeItem.Resource = resource;
						timeItem.Resource.Employee = employee;
						timeItem.Resource.Employee.Specialty = specialty;
						timeItem.Resource.Room = room;
						if (visitItem != null)
						{
							timeItem.VisitItem = visitItem;
							timeItem.VisitItem.TimeItem = timeItem;
						}

						return timeItem;
					},
					param: new { beginDate, endDate, resourceID },
					commandType: CommandType.StoredProcedure
				).AsList();
			}
		}

		public List<TimeItemTotal> GetResourceTotals(DateTime beginDate, DateTime endDate, int specialtyID = 0)
		{
			using (var db = OpenConnection())
			{
				return db.Query<TimeItemTotal>(
					sql: "[dbo].[sp_TimeItems_GetResourceTotals]",
					param: new { beginDate, endDate, specialtyID },
					commandType: CommandType.StoredProcedure
				).AsList();
			}
		}

		public List<TimeItemTotal> GetDispanserizationTotals(DateTime beginDate, DateTime endDate)
		{
			using (var db = OpenConnection())
			{
				return db.Query<TimeItemTotal>(
					sql: "[dbo].[sp_TimeItems_GetDispanserizationTotals]",
					param: new { beginDate, endDate },
					commandType: CommandType.StoredProcedure
				).AsList();
			}
		}
	}
}
