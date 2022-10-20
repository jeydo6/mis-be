using System;
using FluentAssertions;
using MIS.Domain.Entities;
using MIS.Domain.Enums;
using MIS.Persistence.Repositories;
using MIS.Tests.Fixtures.Live;
using Xunit;

namespace MIS.Tests.Repositories;

[Collection("Database collection")]
public class TimeItemsRepositoryTests : IClassFixture<DataFixture>
{
	private readonly DataFixture _fixture;

	public TimeItemsRepositoryTests(DataFixture fixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public void WhenCreate_WithGet_ThenReturnSuccess()
	{
		// Arrange
		var beginDateTime = _fixture.Faker.Date.Soon();

		// Act
		var timeItemsRepository = new TimeItemsRepository(_fixture.ConnectionString);
		var resourcesRepository = new ResourcesRepository(_fixture.ConnectionString);
		var roomsRepository = new RoomsRepository(_fixture.ConnectionString);
		var employeesRepository = new EmployeesRepository(_fixture.ConnectionString);
		var specialtiesRepository = new SpecialtiesRepository(_fixture.ConnectionString);

		var specialtyID = specialtiesRepository.Create(new Specialty
		{
			Code = _fixture.Faker.Random.String2(16),
			Name = _fixture.Faker.Random.String2(10)
		});

		var employeeID = employeesRepository.Create(new Employee
		{
			Code = _fixture.Faker.Random.String2(16),
			FirstName = _fixture.Faker.Random.String2(10),
			MiddleName = _fixture.Faker.Random.String2(10),
			LastName = _fixture.Faker.Random.String2(10),
			SpecialtyID = specialtyID
		});

		var roomID = roomsRepository.Create(new Room
		{
			Code = _fixture.Faker.Random.String2(16),
			Floor = _fixture.Faker.Random.Int(1, 10)
		});

		var resourceID = resourcesRepository.Create(new Resource
		{
			EmployeeID = employeeID,
			RoomID = roomID,
			Type = ResourceType.Doctor,
			Name = _fixture.Faker.Random.String2(10)
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
		var beginDateTime = _fixture.Faker.Date.Soon();

		// Act/Assert
		var timeItemsRepository = new TimeItemsRepository(_fixture.ConnectionString);
		var resourcesRepository = new ResourcesRepository(_fixture.ConnectionString);
		var roomsRepository = new RoomsRepository(_fixture.ConnectionString);
		var employeesRepository = new EmployeesRepository(_fixture.ConnectionString);
		var specialtiesRepository = new SpecialtiesRepository(_fixture.ConnectionString);

		var specialtyID = specialtiesRepository.Create(new Specialty
		{
			Code = _fixture.Faker.Random.String2(16),
			Name = _fixture.Faker.Random.String2(10)
		});

		var employeeID = employeesRepository.Create(new Employee
		{
			Code = _fixture.Faker.Random.String2(16),
			FirstName = _fixture.Faker.Random.String2(10),
			MiddleName = _fixture.Faker.Random.String2(10),
			LastName = _fixture.Faker.Random.String2(10),
			SpecialtyID = specialtyID
		});

		var roomID = roomsRepository.Create(new Room
		{
			Code = _fixture.Faker.Random.String2(16),
			Floor = _fixture.Faker.Random.Int(1, 10)
		});

		var resourceID = resourcesRepository.Create(new Resource
		{
			EmployeeID = employeeID,
			RoomID = roomID,
			Type = ResourceType.Doctor,
			Name = _fixture.Faker.Random.String2(10)
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
