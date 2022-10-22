using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MIS.Domain.Entities;
using MIS.Domain.Enums;
using MIS.Domain.Repositories;
using MIS.Infomat;
using Xunit;

namespace MIS.Tests.Repositories;

[Collection("Database collection")]
public class TimeItemsRepositoryTests : TestClassBase<Startup>
{
	public TimeItemsRepositoryTests(DatabaseFixture<Startup> fixture) : base(fixture) { }

	[Fact]
	public void WhenCreate_WithGet_ThenReturnSuccess()
	{
		// Arrange
		var beginDateTime = Faker.Date.Soon();

		// Act
		var host = CreateHost();
		var timeItemsRepository = host.Services.GetRequiredService<ITimeItemsRepository>();
		var resourcesRepository = host.Services.GetRequiredService<IResourcesRepository>();
		var roomsRepository = host.Services.GetRequiredService<IRoomsRepository>();
		var employeesRepository = host.Services.GetRequiredService<IEmployeesRepository>();
		var specialtiesRepository = host.Services.GetRequiredService<ISpecialtiesRepository>();

		var specialtyID = specialtiesRepository.Create(new Specialty
		{
			Code = Faker.Random.String2(16),
			Name = Faker.Random.String2(10)
		});

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
			Type = ResourceType.Doctor,
			Name = Faker.Random.String2(10)
		});

		var id = timeItemsRepository.Create(new TimeItem
		{
			ResourceID = resourceID,
			Date = beginDateTime.Date,
			BeginDateTime = beginDateTime,
			EndDateTime = beginDateTime.AddMinutes(15),
		});

		// Assert
		var timeItem = timeItemsRepository.Get(id);

		timeItem.Should().NotBeNull();
		timeItem.Date.Should().Be(beginDateTime.Date);
		timeItem.BeginDateTime.Should().BeCloseTo(beginDateTime, TimeSpan.FromSeconds(1));
		timeItem.EndDateTime.Should().BeAfter(beginDateTime);
		timeItem.VisitItem.Should().BeNull();
		timeItem.Resource.Should().NotBeNull();
		timeItem.Resource.ID.Should().Be(resourceID);
		timeItem.Resource.Name.Should().NotBeNull();
		timeItem.Resource.EmployeeID.Should().Be(employeeID);
		timeItem.Resource.Employee.Should().NotBeNull();
		timeItem.Resource.Employee.SpecialtyID.Should().Be(specialtyID);
		timeItem.Resource.Employee.Specialty.Should().NotBeNull();
		timeItem.Resource.Employee.Should().NotBeNull();
		timeItem.Resource.RoomID.Should().Be(roomID);
		timeItem.Resource.Room.Should().NotBeNull();
		timeItem.Resource.Type.Should().BeDefined();
		timeItem.Resource.Type.Should().NotBe(ResourceType.Unknown);
	}

	[Fact]
	public void WhenCreate_WithDuplicate_ThenThrowException()
	{
		// Arrange
		var beginDateTime = Faker.Date.Soon();

		// Act/Assert
		var host = CreateHost();
		var timeItemsRepository = host.Services.GetRequiredService<ITimeItemsRepository>();
		var resourcesRepository = host.Services.GetRequiredService<IResourcesRepository>();
		var roomsRepository = host.Services.GetRequiredService<IRoomsRepository>();
		var employeesRepository = host.Services.GetRequiredService<IEmployeesRepository>();
		var specialtiesRepository = host.Services.GetRequiredService<ISpecialtiesRepository>();

		var specialtyID = specialtiesRepository.Create(new Specialty
		{
			Code = Faker.Random.String2(16),
			Name = Faker.Random.String2(10)
		});

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
			Type = ResourceType.Doctor,
			Name = Faker.Random.String2(10)
		});

		FluentActions
			.Invoking(() => timeItemsRepository.Create(new TimeItem
			{
				ResourceID = resourceID,
				Date = beginDateTime.Date,
				BeginDateTime = beginDateTime,
				EndDateTime = beginDateTime.AddMinutes(15),
			}))
			.Should().NotThrow<Exception>();

		FluentActions
			.Invoking(() => timeItemsRepository.Create(new TimeItem
			{
				ResourceID = resourceID,
				Date = beginDateTime.Date,
				BeginDateTime = beginDateTime,
				EndDateTime = beginDateTime.AddMinutes(15),
			}))
			.Should().Throw<Exception>();
	}
}
