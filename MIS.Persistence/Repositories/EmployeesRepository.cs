using System;
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

		var items = db.Query<Employee, Specialty, Employee>(
			sql: "[dbo].[sp_Employees_Get]",
			map: (employee, specialty) =>
			{
				employee.Specialty = specialty;

				return employee;
			},
			param: new { id },
			commandType: CommandType.StoredProcedure
		).AsList();

		if (items.Count == 0)
		{
			throw new Exception($"Работник с id = {id} не найден");
		}

		return items[0];
	}
}
