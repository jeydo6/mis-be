using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;
using MIS.Tests.Factories;
using Xunit;

namespace MIS.Tests.Repositories;

[Collection("Database collection")]
public class EmployeesRepositoryTests : TestClassBase
{
	public EmployeesRepositoryTests(TestApplicationFactory factory) : base(factory) { }

	[Fact]
	public void WhenCreate_WithGet_ThenReturnSuccess()
	{
		// Arrange
		var code = Faker.Random.String2(16);

		// Act
		var host = CreateHost();
		var employeesRepository = host.Services.GetRequiredService<IEmployeesRepository>();
		var specialtiesRepository = host.Services.GetRequiredService<ISpecialtiesRepository>();

		var specialtyID = specialtiesRepository.Create(new Specialty
		{
			Code = Faker.Random.String2(16),
			Name = Faker.Random.String2(10)
		});

		var id = employeesRepository.Create(new Employee
		{
			Code = code,
			FirstName = Faker.Random.String2(10),
			MiddleName = Faker.Random.String2(10),
			LastName = Faker.Random.String2(10),
			SpecialtyID = specialtyID
		});

		// Assert
		var employee = employeesRepository.Get(id);

		employee.Should().NotBeNull();
		employee.ID.Should().Be(id);
		employee.Code.Should().Be(code);
		employee.SpecialtyID.Should().Be(specialtyID);
		employee.Specialty.Should().NotBeNull();
		employee.FirstName.Should().NotBeNull();
		employee.MiddleName.Should().NotBeNull();
		employee.LastName.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public void WhenCreate_WithDuplicate_ThenThrowException()
	{
		// Arrange
		var code = Faker.Random.String2(16);

		// Act/Assert
		var host = CreateHost();
		var employeesRepository = host.Services.GetRequiredService<IEmployeesRepository>();
		var specialtiesRepository = host.Services.GetRequiredService<ISpecialtiesRepository>();

		var specialtyID = specialtiesRepository.Create(new Specialty
		{
			Code = Faker.Random.String2(16),
			Name = Faker.Random.String2(10)
		});

		FluentActions
			.Invoking(() => employeesRepository.Create(new Employee
			{
				Code = code,
				FirstName = Faker.Random.String2(10),
				MiddleName = Faker.Random.String2(10),
				LastName = Faker.Random.String2(10),
				SpecialtyID = specialtyID
			}))
			.Should().NotThrow<Exception>();

		FluentActions
			.Invoking(() => employeesRepository.Create(new Employee
			{
				Code = code,
				FirstName = Faker.Random.String2(10),
				MiddleName = Faker.Random.String2(10),
				LastName = Faker.Random.String2(10),
				SpecialtyID = specialtyID
			}))
			.Should().Throw<Exception>();
	}

	[Fact]
	public void WhenCreate_WithInvalidSpecialty_ThenThrowException()
	{
		// Arrange
		var code = Faker.Random.String2(16);

		// Act/Assert
		var host = CreateHost();
		var employeesRepository = host.Services.GetRequiredService<IEmployeesRepository>();

		FluentActions
			.Invoking(() => employeesRepository.Create(new Employee
			{
				Code = code,
				FirstName = Faker.Random.String2(10),
				MiddleName = Faker.Random.String2(10),
				LastName = Faker.Random.String2(10),
				SpecialtyID = Faker.Random.Int(1, 1_000_000)
			}))
			.Should().Throw<Exception>();
	}
}
