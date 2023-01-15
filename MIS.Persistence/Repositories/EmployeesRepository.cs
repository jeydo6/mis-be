using System;
using System.Data;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;
using MIS.Persistence.Factories;

namespace MIS.Persistence.Repositories;
public sealed class EmployeesRepository : IEmployeesRepository
{
	private readonly IDbConnectionFactory _connectionFactory;

	public EmployeesRepository(IDbConnectionFactory connectionFactory) =>
		_connectionFactory = connectionFactory;

	public int Create(Employee item)
	{
		using var connection = _connectionFactory.CreateConnection();
		connection.Open();
		
		using var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
		try
		{
			var id = connection.QuerySingle<int>(
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

	public Employee Get(int id)
	{
		using var connection = _connectionFactory.CreateConnection();
		var items = connection.Query<Employee, Specialty, Employee>(
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
