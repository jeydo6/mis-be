using FluentAssertions;
using MIS.Domain.Entities;
using MIS.Domain.Enums;
using MIS.Persistence.Repositories;
using MIS.Tests.Fixtures.Live;
using Xunit;

namespace MIS.Tests.Repositories
{
	[Collection("Collection")]
	public class PatientsRepositoryTests : IClassFixture<DataFixture>
	{
		private readonly DataFixture _fixture;

		public PatientsRepositoryTests(DataFixture fixture)
		{
			_fixture = fixture;
		}

		[Fact]
		public void WhenCreate_WithGet_ThenReturnSuccess()
		{
			// Arrange
			var code = _fixture.Faker.Random.String2(8);
			var birthDate = _fixture.Faker.Date.Past(30).Date;

			// Act
			var patientsRepository = new PatientsRepository(_fixture.ConnectionString);

			var id = patientsRepository.Create(new Patient
			{
				Code = code,
				BirthDate = birthDate,
				FirstName = _fixture.Faker.Random.String2(10),
				MiddleName = _fixture.Faker.Random.String2(10),
				LastName = _fixture.Faker.Random.String2(10),
				Gender = _fixture.Faker.PickRandom<Gender>()
			});

			// Assert
			var patient = patientsRepository.Get(id);

			patient.Should().NotBeNull();
			patient.Code.Should().Be(code);
			patient.BirthDate.Should().Be(birthDate);
			patient.FirstName.Should().NotBeNullOrEmpty();
			patient.MiddleName.Should().NotBeNullOrEmpty();
			patient.LastName.Should().NotBeNullOrEmpty();
			patient.Gender.Should().BeDefined();
		}

		[Fact]
		public void WhenCreate_WithFind_ThenReturnSuccess()
		{
			// Arrange
			var code = _fixture.Faker.Random.String2(8);
			var birthDate = _fixture.Faker.Date.Past(30).Date;

			// Act
			var patientsRepository = new PatientsRepository(_fixture.ConnectionString);

			patientsRepository.Create(new Patient
			{
				Code = code,
				BirthDate = birthDate,
				FirstName = _fixture.Faker.Random.String2(10),
				MiddleName = _fixture.Faker.Random.String2(10),
				LastName = _fixture.Faker.Random.String2(10),
				Gender = _fixture.Faker.PickRandom<Gender>()
			});

			// Assert
			var patient = patientsRepository.First(code, birthDate);

			patient.Should().NotBeNull();
			patient.Code.Should().Be(code);
			patient.BirthDate.Should().Be(birthDate);
			patient.FirstName.Should().NotBeNullOrEmpty();
			patient.MiddleName.Should().NotBeNullOrEmpty();
			patient.LastName.Should().NotBeNullOrEmpty();
			patient.Gender.Should().BeDefined();
		}
	}
}
