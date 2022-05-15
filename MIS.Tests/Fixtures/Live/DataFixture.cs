using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MIS.Domain.Entities;
using MIS.Domain.Providers;
using System;
using System.Collections.Generic;
using System.Data;

namespace MIS.Tests.Fixtures.Live
{
	public class DataFixture : IDisposable
	{
		public DataFixture()
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
				.Build();

			String connectionString = configuration.GetConnectionString("DefaultConnection");

			IDbConnection db = new SqlConnection(connectionString);
			db.Open();

			Transaction = db.BeginTransaction();

			DateTimeProvider = new DefaultDateTimeProvider(new DateTime(2100, 1, 18));

			Seed();
		}

		internal IDateTimeProvider DateTimeProvider { get; }

		internal IDbTransaction Transaction { get; }

		private void Seed()
		{
			Int32 patientID = CreatePatient(new Patient
			{
				Code = "30000000",
				Name = "Иван Иванович",
				BirthDate = new DateTime(1980, 1, 1),
				GenderID = 0,
				Dispanserizations = new List<Dispanserization>(),
				VisitItems = new List<VisitItem>()
			});
			Int32 resourceID = CreateResource(new Resource
			{
				EmployeeID = -1,
				Name = "Врач-терапевт",
				Employee = new Employee
				{
					Code = "1001",
					Name = "Петров П. П.",
					SpecialtyID = -1,
					Specialty = new Specialty
					{
						Code = "1001",
						Name = "Терапия"
					}
				},
				RoomID = -1,
				Room = new Room
				{
					Code = "1001",
					Floor = 1
				}
			});

			List<Int32> timeItemIDs = new List<Int32>();
			for (Int32 i = 0; i < 6; i++)
			{
				timeItemIDs.Add(
					CreateTimeItem(new TimeItem
					{
						Date = DateTimeProvider.Now.Date,
						BeginDateTime = DateTimeProvider.Now.Date.AddHours(8).AddMinutes(i * 15),
						EndDateTime = DateTimeProvider.Now.Date.AddHours(8).AddMinutes(i * 15 + 15),
						ResourceID = resourceID
					})
				);
			}

			CreateVisitItem(new VisitItem
			{
				PatientID = patientID,
				TimeItemID = timeItemIDs[0]
			});

			CreateDispanserization(new Dispanserization
			{
				PatientID = patientID,
				BeginDate = new DateTime(DateTimeProvider.Now.Year - 1, 1, 1),
				EndDate = new DateTime(DateTimeProvider.Now.Year - 1, 12, 31)
			});
		}

		private Int32 CreateDispanserization(Dispanserization item)
		{
			Int32 dispanserizationID = Transaction.Connection.QuerySingle<Int32>(
				sql: "[dbo].[sp_Dispanserizations_Create]",
				param: new
				{
					patientID = item.PatientID,
					beginDate = item.BeginDate,
					endDate = item.EndDate
				},
				commandType: CommandType.StoredProcedure,
				transaction: Transaction
			);

			return dispanserizationID;
		}

		private Int32 CreatePatient(Patient item)
		{
			throw new NotImplementedException();
			//Int32 patientID = Transaction.Connection.QuerySingle<Int32>(
			//	sql: "[dbo].[sp_Patients_Create]",
			//	param: new
			//	{
			//		code = item.Code,
			//		firstName = item.FirstName,
			//		middleName = item.MiddleName,
			//		lastName = item.LastName,
			//		birthDate = item.BirthDate,
			//		genderID = item.Gender
			//	},
			//	commandType: CommandType.StoredProcedure,
			//	transaction: Transaction
			//);

			//return patientID;
		}

		private Int32 CreateResource(Resource item)
		{
			throw new NotImplementedException();
			//Int32 resourceID = Transaction.Connection.QuerySingle<Int32>(
			//	sql: "[dbo].[sp_Resources_Create]",
			//	param: new
			//	{
			//		employeeID = item.EmployeeID,
			//		employeeCode = item.Employee.Code,
			//		employeeFirstName = item.Employee.FirstName,
			//		employeeMiddleName = item.Employee.MiddleName,
			//		employeeLastName = item.Employee.LastName,
			//		specialtyID = item.Employee.SpecialtyID,
			//		specialtyCode = item.Employee.Specialty.Code,
			//		specialtyName = item.Employee.Specialty.Name,
			//		roomID = item.RoomID,
			//		roomCode = item.Room.Code,
			//		roomFloor = item.Room.Floor,
			//	},
			//	commandType: CommandType.StoredProcedure,
			//	transaction: Transaction
			//);

			//return resourceID;
		}

		private Int32 CreateTimeItem(TimeItem item)
		{
			Int32 timeItemID = Transaction.Connection.QuerySingle<Int32>(
				sql: "[dbo].[sp_TimeItems_Create]",
				param: new
				{
					date = item.Date,
					beginDateTime = item.BeginDateTime,
					endDateTime = item.EndDateTime,
					resourceID = item.ResourceID
				},
				commandType: CommandType.StoredProcedure,
				transaction: Transaction
			);

			return timeItemID;
		}

		private Int32 CreateVisitItem(VisitItem item)
		{
			Int32 visitItemID = Transaction.Connection.QuerySingle<Int32>(
				sql: "[dbo].[sp_VisitItems_Create]",
				param: new
				{
					patientID = item.PatientID,
					timeItemID = item.TimeItemID
				},
				commandType: CommandType.StoredProcedure,
				transaction: Transaction
			);

			return visitItemID;
		}

		public void Dispose()
		{
			if (Transaction != null)
			{
				Transaction.Rollback();
				Transaction.Dispose();
			}
		}
	}
}
