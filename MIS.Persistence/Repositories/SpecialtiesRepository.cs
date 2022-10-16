using System.Data;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories;

public sealed class SpecialtiesRepository : BaseRepository, ISpecialtiesRepository
{
	public SpecialtiesRepository(string connectionString) : base(connectionString) { }

	public int Create(Specialty item)
	{
		using (var db = OpenConnection())
		using (var transaction = db.BeginTransaction(IsolationLevel.ReadUncommitted))
		{
			try
			{
				var id = db.QuerySingle<int>(
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
	}

	public Specialty Get(int id)
	{
		using var db = OpenConnection();

		return db.QueryFirstOrDefault<Specialty>(
			sql: "[dbo].[sp_Specialties_Get]",
			param: new { id },
			commandType: CommandType.StoredProcedure
		);
	}

	public Specialty FindByName(string name)
	{
		using var db = OpenConnection();

		return db.QueryFirstOrDefault<Specialty>(
			sql: "[dbo].[sp_Specialties_FindByName]",
			param: new { name },
			commandType: CommandType.StoredProcedure
		);
	}
}
