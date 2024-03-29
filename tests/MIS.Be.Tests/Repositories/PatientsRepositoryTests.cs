﻿using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Enums;
using MIS.Be.Domain.Repositories;
using MIS.Be.Tests.Factories;
using Xunit;

namespace MIS.Be.Tests.Repositories;

[Collection("Database collection")]
public class PatientsRepositoryTests : TestClassBase
{
	public PatientsRepositoryTests(TestApplicationFactory factory) : base(factory) { }

	[Fact]
	public void WhenCreate_WithGet_ThenReturnSuccess()
	{
		// Arrange
		var code = Faker.Random.String2(8);
		var birthDate = Faker.Date.Past(30).Date;

		// Act
		var host = CreateHost();
		var patientsRepository = host.Services.GetRequiredService<IPatientsRepository>();

		var id = patientsRepository.Create(new Patient
		{
			Code = code,
			BirthDate = birthDate,
			FirstName = Faker.Random.String2(10),
			MiddleName = Faker.Random.String2(10),
			LastName = Faker.Random.String2(10),
			Gender = Faker.PickRandomWithout(Gender.Unknown)
		});

		// Assert
		var patient = patientsRepository.Get(id);

		patient.Should().NotBeNull();
		patient.ID.Should().Be(id);
		patient.Code.Should().Be(code);
		patient.BirthDate.Should().Be(birthDate);
		patient.FirstName.Should().NotBeNull();
		patient.MiddleName.Should().NotBeNull();
		patient.LastName.Should().NotBeNullOrEmpty();
		patient.Gender.Should().BeDefined();
		patient.Gender.Should().NotBe(Gender.Unknown);
	}

	[Fact]
	public void WhenCreate_WithFind_ThenReturnSuccess()
	{
		// Arrange
		var code = Faker.Random.String2(8);
		var birthDate = Faker.Date.Past(30).Date;

		// Act
		var host = CreateHost();
		var patientsRepository = host.Services.GetRequiredService<IPatientsRepository>();

		patientsRepository.Create(new Patient
		{
			Code = code,
			BirthDate = birthDate,
			FirstName = Faker.Random.String2(10),
			MiddleName = Faker.Random.String2(10),
			LastName = Faker.Random.String2(10),
			Gender = Faker.PickRandomWithout(Gender.Unknown)
		});

		// Assert
		var patient = patientsRepository.Find(code, birthDate);

		patient.Should().NotBeNull();
		patient.Code.Should().Be(code);
		patient.BirthDate.Should().Be(birthDate);
		patient.FirstName.Should().NotBeNull();
		patient.MiddleName.Should().NotBeNull();
		patient.LastName.Should().NotBeNullOrEmpty();
		patient.Gender.Should().BeDefined();
		patient.Gender.Should().NotBe(Gender.Unknown);
	}

	[Fact]
	public void WhenCreate_WithDuplicate_ThenThrowException()
	{
		// Arrange
		var code = Faker.Random.String2(8);

		// Act/Assert
		var host = CreateHost();
		var patientsRepository = host.Services.GetRequiredService<IPatientsRepository>();

		FluentActions
			.Invoking(() => patientsRepository.Create(new Patient
			{
				Code = code,
				BirthDate = Faker.Date.Past(30).Date,
				FirstName = Faker.Random.String2(10),
				MiddleName = Faker.Random.String2(10),
				LastName = Faker.Random.String2(10),
				Gender = Faker.PickRandomWithout(Gender.Unknown)
			}))
			.Should().NotThrow<Exception>();

		FluentActions
			.Invoking(() => patientsRepository.Create(new Patient
			{
				Code = code,
				BirthDate = Faker.Date.Past(30).Date,
				FirstName = Faker.Random.String2(10),
				MiddleName = Faker.Random.String2(10),
				LastName = Faker.Random.String2(10),
				Gender = Faker.PickRandomWithout(Gender.Unknown)
			}))
			.Should().Throw<Exception>();
	}
}
