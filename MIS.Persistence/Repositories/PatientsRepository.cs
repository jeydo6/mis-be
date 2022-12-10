using System;
using System.Data;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories
{
	public sealed class PatientsRepository : IPatientsRepository
	{
		private readonly IDbConnection _connection;

		public PatientsRepository(IDbConnection connection) =>
			_connection = connection;

		public int Create(Patient item)
		{
			_connection.Open();
			using var transaction = _connection.BeginTransaction(IsolationLevel.ReadUncommitted);
			try
			{
				var id = _connection.QuerySingle<int>(
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
			finally
			{
				_connection.Close();
			}
		}

		public Patient Find(string code, DateTime birthDate)
		{
			return _connection.QueryFirstOrDefault<Patient>(
				sql: "[dbo].[sp_Patients_Find]",
				param: new { code, birthDate },
				commandType: CommandType.StoredProcedure
			);
		}

		public Patient Get(int id)
		{
			var item = _connection.QueryFirstOrDefault<Patient>(
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
