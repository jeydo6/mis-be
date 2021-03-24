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
				FirstName = "Иван",
				MiddleName = "Иванович",
				BirthDate = new DateTime(1980, 1, 1),
				Gender = 0,
				Dispanserizations = new List<Dispanserization>(),
				VisitItems = new List<VisitItem>()
			});
			Int32 resourceID = CreateResource(new Resource
			{
				DoctorID = -1,
				Doctor = new Doctor
				{
					Code = "1001",
					FirstName = "Пётр",
					MiddleName = "Петрович",
					LastName = "Петров",
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
					Flat = 1
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
			Int32 patientID = Transaction.Connection.QuerySingle<Int32>(
				sql: "[dbo].[sp_Patients_Create]",
				param: new
				{
					code = item.Code,
					firstName = item.FirstName,
					middleName = item.MiddleName,
					birthDate = item.BirthDate,
					gender = item.Gender
				},
				commandType: CommandType.StoredProcedure,
				transaction: Transaction
			);

			return patientID;
		}

		private Int32 CreateResource(Resource item)
		{
			Int32 resourceID = Transaction.Connection.QuerySingle<Int32>(
				sql: "[dbo].[sp_Resources_Create]",
				param: new
				{
					doctorID = item.DoctorID,
					doctorCode = item.Doctor.Code,
					doctorFirstName = item.Doctor.FirstName,
					doctorMiddleName = item.Doctor.MiddleName,
					doctorLastName = item.Doctor.LastName,
					specialtyID = item.Doctor.SpecialtyID,
					specialtyCode = item.Doctor.Specialty.Code,
					specialtyName = item.Doctor.Specialty.Name,
					roomID = item.RoomID,
					roomCode = item.Room.Code,
					roomFlat = item.Room.Flat,
				},
				commandType: CommandType.StoredProcedure,
				transaction: Transaction
			);

			return resourceID;
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
