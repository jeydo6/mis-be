using System.Data;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories;

public sealed class SpecialtiesRepository : ISpecialtiesRepository
{
	private readonly IDbConnection _connection;

	public SpecialtiesRepository(IDbConnection connection) =>
		_connection = connection;

	public int Create(Specialty item)
	{
		_connection.Open();
		using var transaction = _connection.BeginTransaction(IsolationLevel.ReadUncommitted);
		try
		{
			var id = _connection.QuerySingle<int>(
				sql: "[dbo].[sp_Specialties_Create]",
				param: new
				{
					code = item.Code,
					name = item.Name
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

	public Specialty Get(int id)
	{
		return _connection.QueryFirstOrDefault<Specialty>(
			sql: "[dbo].[sp_Specialties_Get]",
			param: new { id },
			commandType: CommandType.StoredProcedure
		);
	}

	public Specialty FindByName(string name)
	{
		return _connection.QueryFirstOrDefault<Specialty>(
			sql: "[dbo].[sp_Specialties_FindByName]",
			param: new { name },
			commandType: CommandType.StoredProcedure
		);
	}
}
