﻿using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;
using MIS.Be.Tests.Factories;
using Xunit;

namespace MIS.Be.Tests.Repositories;

[Collection("Database collection")]
public class SpecialtiesRepositoryTests : TestClassBase
{
	public SpecialtiesRepositoryTests(TestApplicationFactory factory) : base(factory) { }

	[Fact]
	public void WhenCreate_WithGet_ThenReturnSuccess()
	{
		// Arrange
		var code = Faker.Random.String2(16);

		// Act
		var host = CreateHost();
		var specialtiesRepository = host.Services.GetRequiredService<ISpecialtiesRepository>();

		var id = specialtiesRepository.Create(new Specialty
		{
			Code = code,
			Name = Faker.Random.String2(10)
		});

		// Assert
		var specialty = specialtiesRepository.Get(id);

		specialty.Should().NotBeNull();
		specialty.ID.Should().Be(id);
		specialty.Code.Should().Be(code);
		specialty.Name.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public void WhenCreate_WithDuplicate_ThenThrowException()
	{
		// Arrange
		var code = Faker.Random.String2(16);

		// Act/Assert
		var host = CreateHost();
		var specialtiesRepository = host.Services.GetRequiredService<ISpecialtiesRepository>();

		FluentActions
			.Invoking(() => specialtiesRepository.Create(new Specialty
			{
				Code = code,
				Name = Faker.Random.String2(10)
			}))
			.Should().NotThrow<Exception>();

		FluentActions
			.Invoking(() => specialtiesRepository.Create(new Specialty
			{
				Code = code,
				Name = Faker.Random.String2(10)
			}))
			.Should().Throw<Exception>();
	}
}
