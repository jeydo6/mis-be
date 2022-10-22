using System;
using System.Collections.Generic;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MIS.Application.Configs;
using MIS.Domain.Entities;
using MIS.Domain.Enums;
using MIS.Domain.Repositories;
using MIS.Mediator;
using MIS.Persistence.Extensions;
using Xunit;

namespace MIS.Tests;

public abstract class TestClassBase : IClassFixture<DatabaseFixture>
{
	private const string DispanserizationSpecialtyName = "Диспансеризация";

	private readonly DatabaseFixture _fixture;

	protected readonly Faker Faker = new Faker();

	public TestClassBase(DatabaseFixture fixture) =>
		_fixture = fixture;

	public IHost CreateHost() => CreateHost(_ => { });

	public IHost CreateHost(Action<IServiceCollection> configuration) =>
		Host
			.CreateDefaultBuilder()
			.ConfigureServices((context, services) =>
			{
				services
					.Configure<SettingsConfig>(
						context.Configuration.GetSection($"{nameof(SettingsConfig)}")
					);

				services
					.AddLogging(l => l.ClearProviders().AddConsole())
					.AddMediator(typeof(Application.AssemblyMarker))
					.ConfigureRelease();
			})
			.ConfigureServices(configuration)
			.Build();

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
