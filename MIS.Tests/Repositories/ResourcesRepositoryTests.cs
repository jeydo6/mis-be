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
public class ResourcesRepositoryTests : TestClassBase<Startup>
{
	public ResourcesRepositoryTests(DatabaseFixture<Startup> fixture) : base(fixture) { }

	[Fact]
	public void WhenCreate_WithGet_ThenReturnSuccess()
	{
		// Arrange
		var name = Faker.Random.String2(10);

		// Act
		var host = CreateHost();
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

		var id = resourcesRepository.Create(new Resource
		{
			Name = name,
			EmployeeID = employeeID,
			RoomID = roomID,
			Type = Faker.PickRandomWithout(ResourceType.Unknown)
		});

		// Assert
		var resource = resourcesRepository.Get(id);

		resource.Should().NotBeNull();
		resource.ID.Should().Be(id);
		resource.Name.Should().Be(name);
		resource.EmployeeID.Should().Be(employeeID);
		resource.Employee.Should().NotBeNull();
		resource.Employee.SpecialtyID.Should().Be(specialtyID);
		resource.Employee.Specialty.Should().NotBeNull();
		resource.Employee.Should().NotBeNull();
		resource.RoomID.Should().Be(roomID);
		resource.Room.Should().NotBeNull();
		resource.Type.Should().BeDefined();
		resource.Type.Should().NotBe(ResourceType.Unknown);
	}

	[Fact]
	public void WhenCreate_WithDuplicate_ThenThrowException()
	{
		// Arrange
		var name = Faker.Random.String2(10);

		// Act/Assert
		var host = CreateHost();
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

		FluentActions
			.Invoking(() => resourcesRepository.Create(new Resource
			{
				Name = name,
				EmployeeID = employeeID,
				RoomID = roomID,
				Type = Faker.PickRandomWithout(ResourceType.Unknown)
			}))
			.Should().NotThrow<Exception>();

		FluentActions
			.Invoking(() => resourcesRepository.Create(new Resource
			{
				Name = name,
				EmployeeID = employeeID,
				RoomID = roomID,
				Type = Faker.PickRandomWithout(ResourceType.Unknown)
			}))
			.Should().Throw<Exception>();
	}
}
