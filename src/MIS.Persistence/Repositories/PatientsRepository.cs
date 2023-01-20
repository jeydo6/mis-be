using System;
using System.Data;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;
using MIS.Persistence.Factories;

namespace MIS.Persistence.Repositories
{
	public sealed class PatientsRepository : IPatientsRepository
	{
		private readonly IDbConnectionFactory _connectionFactory;

		public PatientsRepository(IDbConnectionFactory connectionFactory) =>
			_connectionFactory = connectionFactory;

		public int Create(Patient item)
		{
			using var connection = _connectionFactory.CreateConnection();
			connection.Open();
			using var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
			try
			{
				var id = connection.QuerySingle<int>(
					sql: "[dbo].[sp_Patients_Create]",
					param: new
					{
						code = item.Code,
						firstName = item.FirstName,
						middleName = item.MiddleName,
						lastName = item.LastName,
						birthDate = item.BirthDate,
						gender = item.Gender,
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

		public Patient Find(string code, DateTime birthDate)
		{
			using var connection = _connectionFactory.CreateConnection();
			return connection.QueryFirstOrDefault<Patient>(
				sql: "[dbo].[sp_Patients_Find]",
				param: new { code, birthDate },
				commandType: CommandType.StoredProcedure
			);
		}

		public Patient Get(int id)
		{
			using var connection = _connectionFactory.CreateConnection();
			var item = connection.QueryFirstOrDefault<Patient>(
				sql: "[dbo].[sp_Patients_Get]",
				param: new { id },
				commandType: CommandType.StoredProcedure
			);

			if (item == null)
			{
				throw new Exception($"Пациент с id = {id} не найден");
			}

			return item;
		}
	}
}
