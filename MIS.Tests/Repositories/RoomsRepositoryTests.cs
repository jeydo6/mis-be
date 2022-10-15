using FluentAssertions;
using MIS.Domain.Entities;
using MIS.Persistence.Repositories;
using MIS.Tests.Fixtures.Live;
using Xunit;

namespace MIS.Tests.Repositories;

[Collection("Collection")]
public class RoomsRepositoryTests : IClassFixture<DataFixture>
{
	private readonly DataFixture _fixture;

	public RoomsRepositoryTests(DataFixture fixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public void WhenCreate_WithGet_ThenReturnSuccess()
	{
		// Arrange
		var code = _fixture.Faker.Random.String2(16);

		// Act
		var roomsRepository = new RoomsRepository(_fixture.ConnectionString);

		var id = roomsRepository.Create(new Room
		{
			Code = code,
			Floor = _fixture.Faker.Random.Int(1, 10)
		});

		// Assert
		var room = roomsRepository.Get(id);

		room.Should().NotBeNull();
		room.Code.Should().Be(code);
		room.Floor.Should().BeGreaterThan(0);
	}
}

