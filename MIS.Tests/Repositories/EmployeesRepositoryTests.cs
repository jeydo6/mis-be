using System;
using FluentAssertions;
using MIS.Domain.Entities;
using MIS.Persistence.Repositories;
using MIS.Tests.Fixtures.Live;
using Xunit;

namespace MIS.Tests.Repositories;

[Collection("Database collection")]
public class EmployeesRepositoryTests : IClassFixture<DataFixture>
{
	private readonly DataFixture _fixture;

	public EmployeesRepositoryTests(DataFixture fixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public void WhenCreate_WithGet_ThenReturnSuccess()
	{
		// Arrange
		var code = _fixture.Faker.Random.String2(16);

		// Act
		var employeesRepository = new EmployeesRepository(_fixture.ConnectionString);
		var specialtiesRepository = new SpecialtiesRepository(_fixture.ConnectionString);

		var specialtyID = specialtiesRepository.Create(new Specialty
		{
			Code = _fixture.Faker.Random.String2(16),
			Name = _fixture.Faker.Random.String2(10)
		});

		var id = employeesRepository.Create(new Employee
		{
			Code = code,
			FirstName = _fixture.Faker.Random.String2(10),
			MiddleName = _fixture.Faker.Random.String2(10),
			LastName = _fixture.Faker.Random.String2(10),
			SpecialtyID = specialtyID
		});

		// Assert
		var employee = employeesRepository.Get(id);

		employee.Should().NotBeNull();
		employee.ID.Should().Be(id);
		employee.Code.Should().Be(code);
		employee.SpecialtyID.Should().Be(specialtyID);
		employee.Specialty.Should().NotBeNull();
		employee.FirstName.Should().NotBeNullOrEmpty();
		employee.MiddleName.Should().NotBeNullOrEmpty();
		employee.LastName.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public void WhenCreate_WithDuplicate_ThenThrowException()
	{
		// Arrange
		var code = _fixture.Faker.Random.String2(16);

		// Act/Assert
		var employeesRepository = new EmployeesRepository(_fixture.ConnectionString);
		var specialtiesRepository = new SpecialtiesRepository(_fixture.ConnectionString);

		var specialtyID = specialtiesRepository.Create(new Specialty
		{
			Code = _fixture.Faker.Random.String2(16),
			Name = _fixture.Faker.Random.String2(10)
		});

		FluentActions
			.Invoking(() => employeesRepository.Create(new Employee
			{
				Code = code,
				FirstName = _fixture.Faker.Random.String2(10),
				MiddleName = _fixture.Faker.Random.String2(10),
				LastName = _fixture.Faker.Random.String2(10),
				SpecialtyID = specialtyID
			}))
			.Should().NotThrow<Exception>();

		FluentActions
			.Invoking(() => employeesRepository.Create(new Employee
			{
				Code = code,
				FirstName = _fixture.Faker.Random.String2(10),
				MiddleName = _fixture.Faker.Random.String2(10),
				LastName = _fixture.Faker.Random.String2(10),
				SpecialtyID = specialtyID
			}))
			.Should().Throw<Exception>();
	}

	[Fact]
	public void WhenCreate_WithInvalidSpecialty_ThenThrowException()
	{
		// Arrange
		var code = _fixture.Faker.Random.String2(16);

		// Act/Assert
		var employeesRepository = new EmployeesRepository(_fixture.ConnectionString);

		FluentActions
			.Invoking(() => employeesRepository.Create(new Employee
			{
				Code = code,
				FirstName = _fixture.Faker.Random.String2(10),
				MiddleName = _fixture.Faker.Random.String2(10),
				LastName = _fixture.Faker.Random.String2(10),
				SpecialtyID = _fixture.Faker.Random.Int(1, 1_000_000)
			}))
			.Should().Throw<Exception>();
	}
}
