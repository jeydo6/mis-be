using System;
using System.Data;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories;

public sealed class RoomsRepository : BaseRepository, IRoomsRepository
{
	public RoomsRepository(string connectionString) : base(connectionString) { }

	public int Create(Room item)
	{
		using (var db = OpenConnection())
		using (var transaction = db.BeginTransaction(IsolationLevel.ReadUncommitted))
		{
			try
			{
				var id = db.QuerySingle<int>(
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
	}

	public Room Get(int id)
	{
		using var db = OpenConnection();

		var item = db.QueryFirstOrDefault<Room>(
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
