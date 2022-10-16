using System;
using FluentAssertions;
using MIS.Domain.Entities;
using MIS.Domain.Enums;
using MIS.Persistence.Repositories;
using MIS.Tests.Fixtures.Live;
using Xunit;

namespace MIS.Tests.Repositories;

[Collection("Database collection")]
public class ResourcesRepositoryTests : IClassFixture<DataFixture>
{
	private readonly DataFixture _fixture;

	public ResourcesRepositoryTests(DataFixture fixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public void WhenCreate_WithGet_ThenReturnSuccess()
	{
		// Arrange
		var name = _fixture.Faker.Random.String2(10);

		// Act
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

		var id = resourcesRepository.Create(new Resource
		{
			Name = name,
			EmployeeID = employeeID,
			RoomID = roomID,
			Type = _fixture.Faker.PickRandomWithout(ResourceType.Unknown)
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

	// TODO: Negative scenarios
}
