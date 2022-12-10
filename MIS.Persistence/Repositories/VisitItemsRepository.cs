using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;

namespace MIS.Persistence.Repositories
{
	public class VisitItemsRepository : IVisitItemsRepository
	{
		private readonly IDbConnection _connection;

		public VisitItemsRepository(IDbConnection connection) =>
			_connection = connection;

		public int Create(VisitItem item)
		{
			_connection.Open();
			using var transaction = _connection.BeginTransaction(IsolationLevel.ReadUncommitted);
			try
			{
				var id = _connection.QuerySingle<int>(
					sql: "[dbo].[sp_VisitItems_Create]",
					param: new
					{
						patientID = item.PatientID,
						timeItemID = item.TimeItemID
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

		public VisitItem Get(int visitItemID)
		{
			return _connection.Query<VisitItem, Patient, TimeItem, Resource, Employee, Specialty, Room, VisitItem>(
				sql: "[dbo].[sp_VisitItems_Get]",
				map: (visitItem, patient, timeItem, resource, employee, specialty, room) =>
				{
					visitItem.Patient = patient;
					visitItem.TimeItem = timeItem;
					visitItem.TimeItem.Resource = resource;
					visitItem.TimeItem.Resource.Employee = employee;
					visitItem.TimeItem.Resource.Employee.Specialty = specialty;
					visitItem.TimeItem.Resource.Room = room;
					visitItem.TimeItem.VisitItem = visitItem;

					return visitItem;
				},
				param: new { visitItemID },
				commandType: CommandType.StoredProcedure
			).FirstOrDefault();
		}

		public List<VisitItem> ToList(DateTime beginDate, DateTime endDate, int patientID = 0)
		{
			return _connection.Query<VisitItem, TimeItem, Resource, Employee, Specialty, Room, VisitItem>(
				sql: "[dbo].[sp_VisitItems_List]",
				map: (visitItem, timeItem, resource, employee, specialty, room) =>
				{
					visitItem.TimeItem = timeItem;
					visitItem.TimeItem.Resource = resource;
					visitItem.TimeItem.Resource.Employee = employee;
					visitItem.TimeItem.Resource.Employee.Specialty = specialty;
					visitItem.TimeItem.Resource.Room = room;
					visitItem.TimeItem.VisitItem = visitItem;

					return visitItem;
				},
				param: new { beginDate, endDate, patientID },
				commandType: CommandType.StoredProcedure
			).AsList();
		}
	}
}
