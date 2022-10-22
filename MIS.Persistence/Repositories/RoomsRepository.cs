using System;
using System.Data;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories;

public sealed class RoomsRepository : IRoomsRepository
{
	private readonly IDbConnection _connection;

	public RoomsRepository(IDbConnection connection) =>
		_connection = connection;

	public int Create(Room item)
	{
		_connection.Open();
		using var transaction = _connection.BeginTransaction(IsolationLevel.ReadUncommitted);
		try
		{
			var id = _connection.QuerySingle<int>(
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
		finally
		{
			_connection.Close();
		}
	}

	public Room Get(int id)
	{
		var item = _connection.QueryFirstOrDefault<Room>(
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
