﻿using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories
{
	public class TimeItemsRepository : ITimeItemsRepository
	{
		private readonly IDbConnection _connection;

		public TimeItemsRepository(IDbConnection connection) =>
			_connection = connection;

		public int Create(TimeItem item)
		{
			_connection.Open();
			using var transaction = _connection.BeginTransaction(IsolationLevel.ReadUncommitted);
			try
			{
				var id = _connection.QuerySingle<int>(
					sql: "[dbo].[sp_TimeItems_Create]",
					param: new
					{
						date = item.Date,
						beginDateTime = item.BeginDateTime,
						endDateTime = item.EndDateTime,
						resourceID = item.ResourceID
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

		public TimeItem Get(int id)
		{
			var timeItems = new Dictionary<int, TimeItem>();

			var items = _connection.Query<TimeItem, Resource, Employee, Specialty, Room, VisitItem, TimeItem>(
				sql: "[dbo].[sp_TimeItems_Get]",
				map: (timeItem, resource, employee, specialty, room, visitItem) =>
				{
					if (!timeItems.ContainsKey(timeItem.ID))
					{
						timeItems[timeItem.ID] = timeItem;
					}

					var result = timeItems[timeItem.ID];

					result.Resource = resource;
					result.Resource.Employee = employee;
					result.Resource.Employee.Specialty = specialty;
					result.Resource.Room = room;
					if (visitItem != null)
					{
						result.VisitItem = visitItem;
						result.VisitItem.TimeItem = timeItem;
					}

					return timeItem;
				},
				param: new { id },
				commandType: CommandType.StoredProcedure
			).AsList();

			if (!timeItems.ContainsKey(id))
			{
				throw new Exception($"Слот для записи с id = {id} не найден");
			}

			return timeItems[id];
		}

		public List<TimeItem> ToList(DateTime beginDate, DateTime endDate, int resourceID = 0)
		{
			return _connection.Query<TimeItem, Resource, Employee, Specialty, Room, VisitItem, TimeItem>(
				sql: "[dbo].[sp_TimeItems_List]",
				map: (timeItem, resource, employee, specialty, room, visitItem) =>
				{
					timeItem.Resource = resource;
					timeItem.Resource.Employee = employee;
					timeItem.Resource.Employee.Specialty = specialty;
					timeItem.Resource.Room = room;
					if (visitItem != null)
					{
						timeItem.VisitItem = visitItem;
						timeItem.VisitItem.TimeItem = timeItem;
					}

					return timeItem;
				},
				param: new { beginDate, endDate, resourceID },
				commandType: CommandType.StoredProcedure
			).AsList();
		}

		public List<TimeItemTotal> GetResourceTotals(DateTime beginDate, DateTime endDate, int specialtyID = 0)
		{
			return _connection.Query<TimeItemTotal>(
				sql: "[dbo].[sp_TimeItems_GetResourceTotals]",
				param: new { beginDate, endDate, specialtyID },
				commandType: CommandType.StoredProcedure
			).AsList();
		}

		public List<TimeItemTotal> GetDispanserizationTotals(DateTime beginDate, DateTime endDate)
		{
			return _connection.Query<TimeItemTotal>(
				sql: "[dbo].[sp_TimeItems_GetDispanserizationTotals]",
				param: new { beginDate, endDate },
				commandType: CommandType.StoredProcedure
			).AsList();
		}
	}
}
