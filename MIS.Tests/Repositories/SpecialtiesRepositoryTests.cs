using System;
using FluentAssertions;
using MIS.Domain.Entities;
using MIS.Persistence.Repositories;
using MIS.Tests.Fixtures.Live;
using Xunit;

namespace MIS.Tests.Repositories;

[Collection("Database collection")]
public class SpecialtiesRepositoryTests : IClassFixture<DataFixture>
{
	private readonly DataFixture _fixture;

	public SpecialtiesRepositoryTests(DataFixture fixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public void WhenCreate_WithGet_ThenReturnSuccess()
	{
		// Arrange
		var code = _fixture.Faker.Random.String2(16);

		// Act
		var specialtiesRepository = new SpecialtiesRepository(_fixture.ConnectionString);

		var id = specialtiesRepository.Create(new Specialty
		{
			Code = code,
			Name = _fixture.Faker.Random.String2(10)
		});

		// Assert
		var specialty = specialtiesRepository.Get(id);

		specialty.Should().NotBeNull();
		specialty.Code.Should().Be(code);
		specialty.Name.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public void WhenCreate_WithDuplicate_ThenThrowException()
	{
		// Arrange
		var code = _fixture.Faker.Random.String2(16);

		// Act/Assert
		var specialtiesRepository = new SpecialtiesRepository(_fixture.ConnectionString);

		FluentActions
			.Invoking(() => specialtiesRepository.Create(new Specialty
			{
				Code = code,
				Name = _fixture.Faker.Random.String2(10)
			}))
			.Should().NotThrow<Exception>();

		FluentActions
			.Invoking(() => specialtiesRepository.Create(new Specialty
			{
				Code = code,
				Name = _fixture.Faker.Random.String2(10)
			}))
			.Should().Throw<Exception>();
	}
}
