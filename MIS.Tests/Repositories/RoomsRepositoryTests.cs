using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;
using MIS.Infomat;
using Xunit;

namespace MIS.Tests.Repositories;

[Collection("Database collection")]
public class RoomsRepositoryTests : TestClassBase<Startup>
{
	public RoomsRepositoryTests(DatabaseFixture<Startup> fixture) : base(fixture) { }

	[Fact]
	public void WhenCreate_WithGet_ThenReturnSuccess()
	{
		// Arrange
		var code = Faker.Random.String2(16);

		// Act
		var host = CreateHost();
		var roomsRepository = host.Services.GetRequiredService<IRoomsRepository>();

		var id = roomsRepository.Create(new Room
		{
			Code = code,
			Floor = Faker.Random.Int(1, 10)
		});

		// Assert
		var room = roomsRepository.Get(id);

		room.Should().NotBeNull();
		room.ID.Should().Be(id);
		room.Code.Should().Be(code);
		room.Floor.Should().BeGreaterThan(0);
	}

	[Fact]
	public void WhenCreate_WithDuplicate_ThenThrowException()
	{
		// Arrange
		var code = Faker.Random.String2(16);

		// Act/Assert
		var host = CreateHost();
		var roomsRepository = host.Services.GetRequiredService<IRoomsRepository>();

		FluentActions
			.Invoking(() => roomsRepository.Create(new Room
			{
				Code = code,
				Floor = Faker.Random.Int(1, 10)
			}))
			.Should().NotThrow<Exception>();

		FluentActions
			.Invoking(() => roomsRepository.Create(new Room
			{
				Code = code,
				Floor = Faker.Random.Int(1, 10)
			}))
			.Should().Throw<Exception>();
	}
}

