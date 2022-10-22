using System;
using System.Data;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories;
public sealed class EmployeesRepository : IEmployeesRepository
{
	private readonly IDbConnection _connection;

	public EmployeesRepository(IDbConnection connection) =>
		_connection = connection;

	public int Create(Employee item)
	{
		_connection.Open();
		using var transaction = _connection.BeginTransaction(IsolationLevel.ReadUncommitted);
		try
		{
			var id = _connection.QuerySingle<int>(
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
		finally
		{
			_connection.Close();
		}
	}

	public Employee Get(int id)
	{
		var items = _connection.Query<Employee, Specialty, Employee>(
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
