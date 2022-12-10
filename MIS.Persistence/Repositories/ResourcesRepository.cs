using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories
{
	public class ResourcesRepository : IResourcesRepository
	{
		private readonly IDbConnection _connection;

		public ResourcesRepository(IDbConnection connection) =>
			_connection = connection;

		public int Create(Resource item)
		{
			_connection.Open();
			using var transaction = _connection.BeginTransaction(IsolationLevel.ReadUncommitted);
			try
			{
				var id = _connection.QuerySingle<int>(
					sql: "[dbo].[sp_Resources_Create]",
					param: new
					{
						name = item.Name,
						type = item.Type,
						isDispanserization = false,
						employeeID = item.EmployeeID,
						roomID = item.RoomID
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
			finally
			{
				_connection.Close();
			}
		}

		public Resource Get(int id)
		{
			var items = _connection.Query<Resource, Employee, Specialty, Room, Resource>(
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
			return _connection.Query<Resource, Employee, Specialty, Room, Resource>(
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

		public int CreateDispanserization(Resource item)
		{
			_connection.Open();
			using var transaction = _connection.BeginTransaction(IsolationLevel.ReadUncommitted);
			try
			{
				var id = _connection.QuerySingle<int>(
					sql: "[dbo].[sp_Resources_Create]",
					param: new
					{
						name = item.Name,
						type = item.Type,
						isDispanserization = true,
						employeeID = item.EmployeeID,
						roomID = item.RoomID
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
			finally
			{
				_connection.Close();
			}
		}

		public List<Resource> GetDispanserizations()
		{
			return _connection.Query<Resource, Employee, Specialty, Room, Resource>(
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
