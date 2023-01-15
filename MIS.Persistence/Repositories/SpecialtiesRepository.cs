using System.Data;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;
using MIS.Persistence.Factories;

namespace MIS.Persistence.Repositories;

public sealed class SpecialtiesRepository : ISpecialtiesRepository
{
	private readonly IDbConnectionFactory _connectionFactory;

	public SpecialtiesRepository(IDbConnectionFactory connectionFactory) =>
		_connectionFactory = connectionFactory;

	public int Create(Specialty item)
	{
		using var connection = _connectionFactory.CreateConnection();
		connection.Open();
		
		using var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
		try
		{
			var id = connection.QuerySingle<int>(
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
	}

	public Specialty Get(int id)
	{
		using var connection = _connectionFactory.CreateConnection();
		
		return connection.QueryFirstOrDefault<Specialty>(
			sql: "[dbo].[sp_Specialties_Get]",
			param: new { id },
			commandType: CommandType.StoredProcedure
		);
	}
}
