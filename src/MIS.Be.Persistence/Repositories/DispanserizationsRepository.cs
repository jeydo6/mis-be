using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;
using MIS.Be.Persistence.Factories;

namespace MIS.Be.Persistence.Repositories
{
	public sealed class DispanserizationsRepository : IDispanserizationsRepository
	{
		private readonly IDbConnectionFactory _connectionFactory;

		public DispanserizationsRepository(IDbConnectionFactory connectionFactory) =>
			_connectionFactory = connectionFactory;

		public int Create(Dispanserization item)
		{
			using var connection = _connectionFactory.CreateConnection();
			connection.Open();
			
			using var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
			try
			{
				var id = connection.QuerySingle<int>(
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
		}

		public Dispanserization Get(int id)
		{
			using var connection = _connectionFactory.CreateConnection();
			
			var dispanserizations = new Dictionary<int, Dispanserization>();

			var items = connection.Query<Dispanserization, Research, Dispanserization>(
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
			using var connection = _connectionFactory.CreateConnection();
			
			var dispanserizations = new Dictionary<int, Dispanserization>();

			var items = connection.Query<Dispanserization, Research, Dispanserization>(
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
