using System;
using System.Collections.Generic;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MIS.Application.Startups;
using MIS.Domain.Entities;
using MIS.Domain.Enums;
using MIS.Domain.Repositories;
using Xunit;

namespace MIS.Tests;

public abstract class TestClassBase<TStartup> : IClassFixture<DatabaseFixture<TStartup>>, ITestApplicationFactoryFixture<TStartup>
	where TStartup : StartupBase
{
	private const string DispanserizationSpecialtyName = "Диспансеризация";

	private readonly ITestApplicationFactoryFixture<TStartup> _fixture;

	protected readonly Faker Faker = new Faker();

	public TestClassBase(DatabaseFixture<TStartup> fixture) =>
		_fixture = fixture;

	public IHost CreateHost() =>
		_fixture.CreateHost();

	public IHost CreateHost(Action<IServiceCollection> configuration) =>
		_fixture.CreateHost(configuration);

	internal int[] CreateDispanserizationResources()
	{
		var host = CreateHost();

		var resourcesRepository = host.Services.GetRequiredService<IResourcesRepository>();
		var roomsRepository = host.Services.GetRequiredService<IRoomsRepository>();
		var employeesRepository = host.Services.GetRequiredService<IEmployeesRepository>();
		var specialtiesRepository = host.Services.GetRequiredService<ISpecialtiesRepository>();

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
			var specialty = specialtiesRepository.FindByName(DispanserizationSpecialtyName);
			var specialtyID = specialty != null ?
				specialty.ID :
				specialtiesRepository.Create(new Specialty
				{
					Code = Faker.Random.String2(16),
					Name = DispanserizationSpecialtyName
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
		var host = CreateHost();

		var timeItemsRepository = host.Services.GetRequiredService<ITimeItemsRepository>();

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
