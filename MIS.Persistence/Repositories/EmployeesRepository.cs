using System.Data;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories;
public sealed class EmployeesRepository : BaseRepository, IEmployeesRepository
{
	public EmployeesRepository(string connectionString) : base(connectionString) { }

	public int Create(Employee item)
	{
		using (var db = OpenConnection())
		using (var transaction = db.BeginTransaction(IsolationLevel.ReadUncommitted))
		{
			try
			{
				var id = db.QuerySingle<int>(
					sql: "[dbo].[sp_Employees_Create]",
					param: new
					{
						code = item.Code,
						firstName = item.FirstName,
						middleName = item.MiddleName,
						lastName = item.LastName,
						specialtyID = item.SpecialtyID
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

	public Employee Get(int id)
	{
		using var db = OpenConnection();

		return db.QueryFirstOrDefault<Employee>(
			sql: "[dbo].[sp_Employees_Get]",
			param: new { id },
			commandType: CommandType.StoredProcedure
		);
	}
}
