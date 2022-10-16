using System;
using System.Collections.Generic;
using Bogus;
using Microsoft.Extensions.Configuration;
using MIS.Domain.Entities;
using MIS.Domain.Enums;
using MIS.Domain.Providers;
using MIS.Persistence.Repositories;

namespace MIS.Tests.Fixtures.Live
{
	public class DataFixture
	{
		private const string SpecialtyName = "Диспансеризация";

		public DataFixture()
		{
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
				.Build();

			ConnectionString = configuration.GetConnectionString("DefaultConnection");
			DateTimeProvider = new DefaultDateTimeProvider(new DateTime(2100, 1, 18));
			Faker = new Faker();
		}

		protected internal Faker Faker;

		protected internal IDateTimeProvider DateTimeProvider { get; }

		protected internal string ConnectionString { get; }

		internal int[] CreateDispanserizationResources()
		{
			var resourcesRepository = new ResourcesRepository(ConnectionString);
			var roomsRepository = new RoomsRepository(ConnectionString);
			var employeesRepository = new EmployeesRepository(ConnectionString);
			var specialtiesRepository = new SpecialtiesRepository(ConnectionString);

			var dispanserizationResourceIDs = new List<int>();

			var dispanserizationResources = resourcesRepository.GetDispanserizations();
			if (dispanserizationResources.Count > 0)
			{
				foreach (var resource in dispanserizationResources)
				{
					dispanserizationResourceIDs.Add(resource.ID);
				}
			}
			else
			{
				var specialty = specialtiesRepository.FindByName(SpecialtyName);
				var specialtyID = specialty != null ?
					specialty.ID :
					specialtiesRepository.Create(new Specialty
					{
						Code = Faker.Random.String2(16),
						Name = SpecialtyName
					});

				for (var i = 0; i < 2; i++)
				{
					var employeeID = employeesRepository.Create(new Employee
					{
						Code = Faker.Random.String2(16),
						FirstName = Faker.Random.String2(10),
						MiddleName = Faker.Random.String2(10),
						LastName = Faker.Random.String2(10),
						SpecialtyID = specialtyID
					});

					var roomID = roomsRepository.Create(new Room
					{
						Code = Faker.Random.String2(16),
						Floor = Faker.Random.Int(1, 10)
					});

					var resourceID = resourcesRepository.Create(new Resource
					{
						EmployeeID = employeeID,
						RoomID = roomID,
						Type = ResourceType.Laboratory,
						Name = Faker.Random.String2(10)
					});

					dispanserizationResourceIDs.Add(resourceID);
				}
			}

			return dispanserizationResourceIDs.ToArray();
		}

		internal int[] CreateDispanserizationTimeItems(int[] dispanserizationResourceIDs)
		{
			var timeItemsRepository = new TimeItemsRepository(ConnectionString);

			var timeItemIDs = new List<int>();

			foreach (var resourceID in dispanserizationResourceIDs)
			{
				var beginDateTime = Faker.Date.Soon();

				var timeItemID = timeItemsRepository.Create(new TimeItem
				{
					ResourceID = resourceID,
					Date = beginDateTime.Date,
					BeginDateTime = beginDateTime,
					EndDateTime = beginDateTime.AddMinutes(15),
				});

				timeItemIDs.Add(timeItemID);
			}

			return timeItemIDs.ToArray();
		}
	}
}
