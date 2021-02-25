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

namespace MIS.Persistence.Repositories
{
	public class ResourcesRepository : IResourcesRepository, IDisposable
	{
		private readonly IDbConnection _db;
		private readonly IDbTransaction _transaction;

		public ResourcesRepository(String connectionString)
		{
			_db = new SqlConnection(connectionString);
			_transaction = null;
		}

		public ResourcesRepository(IDbTransaction transaction)
		{
			_db = transaction.Connection;
			_transaction = transaction;
		}

		public IEnumerable<Resource> ToList()
		{
			IEnumerable<Resource> resources = _db.Query<Resource, Doctor, Specialty, Room, Resource>(
				sql: "[dbo].[sp_Resources_List]",
				map: (resource, doctor, specialty, room) =>
				{
					resource.Doctor = doctor;
					resource.Doctor.Specialty = specialty;
					resource.Room = room;

					return resource;
				},
				commandType: CommandType.StoredProcedure,
				transaction: _transaction
			);

			return resources;
		}

		public IEnumerable<Resource> GetDispanserizations()
		{
			IEnumerable<Resource> resources = _db.Query<Resource, Doctor, Specialty, Room, Resource>(
				sql: "[dbo].[sp_Resources_GetDispanserizations]",
				map: (resource, doctor, specialty, room) =>
				{
					resource.Doctor = doctor;
					resource.Doctor.Specialty = specialty;
					resource.Room = room;

					return resource;
				},
				commandType: CommandType.StoredProcedure,
				transaction: _transaction
			);

			return resources;
		}

		public void Dispose()
		{
			if (_db != null)
			{
				_db.Dispose();
			}
		}
	}
}