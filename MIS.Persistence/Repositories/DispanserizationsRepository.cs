using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories
{
	public sealed class DispanserizationsRepository : IDispanserizationsRepository
	{
		private readonly IDbConnection _connection;

		public DispanserizationsRepository(IDbConnection connection) =>
			_connection = connection;

		public int Create(Dispanserization item)
		{
			_connection.Open();
			using var transaction = _connection.BeginTransaction(IsolationLevel.ReadUncommitted);
			try
			{
				var id = _connection.QuerySingle<int>(
					sql: "[dbo].[sp_Dispanserizations_Create]",
					param: new
					{
						patientID = item.PatientID,
						beginDate = item.BeginDate,
						endDate = item.EndDate
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

		public Dispanserization Get(int id)
		{
			var dispanserizations = new Dictionary<int, Dispanserization>();

			var items = _connection.Query<Dispanserization, Research, Dispanserization>(
				sql: "[dbo].[sp_Dispanserizations_Get]",
				map: (dispanserization, research) =>
				{
					if (!dispanserizations.ContainsKey(dispanserization.ID))
					{
						dispanserizations[dispanserization.ID] = dispanserization;
					}

					var result = dispanserizations[dispanserization.ID];
					result.Researches.Add(research);

					return result;
				},
				param: new { id },
				commandType: CommandType.StoredProcedure
			).AsList();

			if (!dispanserizations.ContainsKey(id))
			{
				throw new Exception($"Диспансеризация с id = {id} не найдена");
			}

			return dispanserizations[id];
		}

		public List<Dispanserization> ToList(int patientID)
		{
			var dispanserizations = new Dictionary<int, Dispanserization>();

			var items = _connection.Query<Dispanserization, Research, Dispanserization>(
					sql: "[dbo].[sp_Dispanserizations_List]",
					map: (dispanserization, research) =>
					{
						if (!dispanserizations.TryGetValue(dispanserization.ID, out var value))
						{
							value = dispanserization;
							value.Researches = new List<Research>();
							dispanserizations[dispanserization.ID] = dispanserization;
						}

						value.Researches.Add(research);
						return value;
					},
					param: new { patientID },
					commandType: CommandType.StoredProcedure
				).AsList();

			return new List<Dispanserization>(dispanserizations.Values);
		}
	}
}
