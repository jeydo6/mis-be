using System;
using System.Data;
using Dapper;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;
using MIS.Be.Persistence.Factories;

namespace MIS.Be.Persistence.Repositories;

public sealed class RoomsRepository : IRoomsRepository
{
	private readonly IDbConnectionFactory _connectionFactory;

	public RoomsRepository(IDbConnectionFactory connectionFactory) =>
		_connectionFactory = connectionFactory;

	public int Create(Room item)
	{
		using var connection = _connectionFactory.CreateConnection();
		connection.Open();
		
		using var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
		try
		{
			var id = connection.QuerySingle<int>(
				sql: "[dbo].[sp_Rooms_Create]",
				param: new
				{
					code = item.Code,
					floor = item.Floor
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

	public Room Get(int id)
	{
		using var connection = _connectionFactory.CreateConnection();
		
		var item = connection.QueryFirstOrDefault<Room>(
			sql: "[dbo].[sp_Rooms_Get]",
			param: new { id },
			commandType: CommandType.StoredProcedure
		);

		if (item == null)
		{
			throw new Exception($"Помещение с id = {id} не найдено");
		}

		return item;
	}
}
