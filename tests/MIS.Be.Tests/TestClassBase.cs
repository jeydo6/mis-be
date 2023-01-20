using System;
using System.Collections.Generic;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Enums;
using MIS.Be.Domain.Repositories;
using MIS.Be.Tests.Factories;

namespace MIS.Be.Tests;

public abstract class TestClassBase : IApplicationFactory
{
	private const int DispanserizationResourcesCount = 2;

	private readonly IApplicationFactory _factory;

	protected readonly Faker Faker = new Faker();

	public TestClassBase(TestApplicationFactory factory) =>
		_factory = factory;

	public IHost CreateHost() =>
		_factory.CreateHost();

	public IHost CreateHost(Action<IServiceCollection> configuration) =>
		_factory.CreateHost(configuration);

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
			var specialtyID = specialtiesRepository.Create(new Specialty
			{
				Code = Faker.Random.String2(16),
				Name = Faker.Random.String2(10)
			});

			for (var i = 0; i < DispanserizationResourcesCount; i++)
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

				var resourceID = resourcesRepository.CreateDispanserization(new Resource
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

	internal int[] CreateTimeItems(int[] resourceIDs, DateTime beginDateTime)
	{
		var host = CreateHost();

		var timeItemsRepository = host.Services.GetRequiredService<ITimeItemsRepository>();

		var timeItemIDs = new List<int>();

		foreach (var resourceID in resourceIDs)
		{
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
