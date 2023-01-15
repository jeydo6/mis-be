using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;
using MIS.Persistence.Factories;

namespace MIS.Persistence.Repositories
{
	public class VisitItemsRepository : IVisitItemsRepository
	{
		private readonly IDbConnectionFactory _connectionFactory;

		public VisitItemsRepository(IDbConnectionFactory connectionFactory) =>
			_connectionFactory = connectionFactory;

		public int Create(VisitItem item)
		{
			using var connection = _connectionFactory.CreateConnection();
			connection.Open();
			
			using var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
			try
			{
				var id = connection.QuerySingle<int>(
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
		}

		public VisitItem Get(int visitItemID)
		{
			using var connection = _connectionFactory.CreateConnection();
			return connection.Query<VisitItem, Patient, TimeItem, Resource, Employee, Specialty, Room, VisitItem>(
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
			using var connection = _connectionFactory.CreateConnection();
			return connection.Query<VisitItem, TimeItem, Resource, Employee, Specialty, Room, VisitItem>(
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
