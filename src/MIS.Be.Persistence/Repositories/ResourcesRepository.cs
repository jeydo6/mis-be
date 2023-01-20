using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;
using MIS.Be.Persistence.Factories;

namespace MIS.Be.Persistence.Repositories
{
	public class ResourcesRepository : IResourcesRepository
	{
		private readonly IDbConnectionFactory _connectionFactory;

		public ResourcesRepository(IDbConnectionFactory connectionFactory) =>
			_connectionFactory = connectionFactory;

		public int Create(Resource item)
		{
			using var connection = _connectionFactory.CreateConnection();
			connection.Open();
			
			using var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
			try
			{
				var id = connection.QuerySingle<int>(
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
				connection.Close();
			}
		}

		public Resource Get(int id)
		{
			using var connection = _connectionFactory.CreateConnection();

			var items = connection.Query<Resource, Employee, Specialty, Room, Resource>(
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
			using var connection = _connectionFactory.CreateConnection();
			
			return connection.Query<Resource, Employee, Specialty, Room, Resource>(
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
			using var connection = _connectionFactory.CreateConnection();
			connection.Open();
			
			using var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
			try
			{
				var id = connection.QuerySingle<int>(
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
		}

		public List<Resource> GetDispanserizations()
		{
			using var connection = _connectionFactory.CreateConnection();
			
			return connection.Query<Resource, Employee, Specialty, Room, Resource>(
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
