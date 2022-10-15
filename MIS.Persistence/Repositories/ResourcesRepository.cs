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
	public class ResourcesRepository : BaseRepository, IResourcesRepository
	{
		public ResourcesRepository(string connectionString) : base(connectionString) { }

		public int Create(Resource item)
		{
			using (var db = OpenConnection())
			using (var transaction = db.BeginTransaction(IsolationLevel.ReadUncommitted))
			{
				try
				{
					var resourceID = db.QuerySingle<int>(
						sql: "[dbo].[sp_Resources_Create]",
						param: new
						{
							name = item.Name,
							type = item.Type,
							employeeID = item.EmployeeID,
							roomID = item.RoomID
						},
						commandType: CommandType.StoredProcedure,
						transaction: transaction
					);

					transaction.Commit();
					return resourceID;
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public Resource Get(int id)
		{
			using var db = OpenConnection();

			var items = db.Query<Resource, Employee, Specialty, Room, Resource>(
				sql: "[dbo].[sp_Resources_Get]",
				map: (resource, employee, specialty, room) =>
				{
					resource.Employee = employee;
					resource.Employee.Specialty = specialty;
					resource.Room = room;

					return resource;
				},
				param: new { id },
				commandType: CommandType.StoredProcedure
			).AsList();

			if (items.Count == 0)
			{
				throw new Exception($"Ресурс с id = {id} не найден");
			}

			return items[0];
		}

		public List<Resource> ToList()
		{
			using (var db = OpenConnection())
			{
				return db.Query<Resource, Employee, Specialty, Room, Resource>(
					sql: "[dbo].[sp_Resources_List]",
					map: (resource, employee, specialty, room) =>
					{
						resource.Employee = employee;
						resource.Employee.Specialty = specialty;
						resource.Room = room;

						return resource;
					},
					commandType: CommandType.StoredProcedure
				).AsList();
			}
		}

		public List<Resource> GetDispanserizations()
		{
			using (var db = OpenConnection())
			{
				return db.Query<Resource, Employee, Specialty, Room, Resource>(
					sql: "[dbo].[sp_Resources_GetDispanserizations]",
					map: (resource, employee, specialty, room) =>
					{
						resource.Employee = employee;
						resource.Employee.Specialty = specialty;
						resource.Room = room;

						return resource;
					},
					commandType: CommandType.StoredProcedure
					).AsList();
			}
		}
	}
}