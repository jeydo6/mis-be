using System;
using FluentAssertions;
using MIS.Domain.Entities;
using MIS.Domain.Enums;
using MIS.Persistence.Repositories;
using MIS.Tests.Fixtures.Live;
using Xunit;

namespace MIS.Tests.Repositories;

[Collection("Database collection")]
public class DispanserizationsRepositoryTests : IClassFixture<DataFixture>
{
	private readonly DataFixture _fixture;

	public DispanserizationsRepositoryTests(DataFixture fixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public void WhenCreate_WithGet_ThenReturnSuccess()
	{
		// Arrange
		var beginDate = _fixture.Faker.Date.Soon().Date;

		// Act
		var dispanserizationsRepository = new DispanserizationsRepository(_fixture.ConnectionString);
		var patientsRepository = new PatientsRepository(_fixture.ConnectionString);

		var dispanserizationResourcesIDs = _fixture.CreateDispanserizationResources();
		_fixture.CreateDispanserizationTimeItems(dispanserizationResourcesIDs);

		var patientID = patientsRepository.Create(new Patient
		{
			Code = _fixture.Faker.Random.String2(8),
			BirthDate = _fixture.Faker.Date.Past(30).Date,
			FirstName = _fixture.Faker.Random.String2(10),
			MiddleName = _fixture.Faker.Random.String2(10),
			LastName = _fixture.Faker.Random.String2(10),
			Gender = _fixture.Faker.PickRandomWithout(Gender.Unknown)
		});

		var id = dispanserizationsRepository.Create(new Dispanserization
		{
			PatientID = patientID,
			BeginDate = beginDate,
			EndDate = _fixture.Faker.Date.Soon(refDate: beginDate),
			IsClosed = _fixture.Faker.Random.Bool()
		});

		// Assert
		var dispanserization = dispanserizationsRepository.Get(id);

		dispanserization.Should().NotBeNull();
		dispanserization.ID.Should().Be(id);
		dispanserization.Researches.Should().HaveSameCount(dispanserizationResourcesIDs);
	}

	[Fact]
	public void WhenCreate_WithDuplicate_ThenThrowException()
	{
		// Arrange
		var beginDate = _fixture.Faker.Date.Soon().Date;

		// Act/Assert
		var dispanserizationsRepository = new DispanserizationsRepository(_fixture.ConnectionString);
		var patientsRepository = new PatientsRepository(_fixture.ConnectionString);

		var dispanserizationResourcesIDs = _fixture.CreateDispanserizationResources();
		_fixture.CreateDispanserizationTimeItems(dispanserizationResourcesIDs);

		var patientID = patientsRepository.Create(new Patient
		{
			Code = _fixture.Faker.Random.String2(8),
			BirthDate = _fixture.Faker.Date.Past(30).Date,
			FirstName = _fixture.Faker.Random.String2(10),
			MiddleName = _fixture.Faker.Random.String2(10),
			LastName = _fixture.Faker.Random.String2(10),
			Gender = _fixture.Faker.PickRandomWithout(Gender.Unknown)
		});

		FluentActions
			.Invoking(() => dispanserizationsRepository.Create(new Dispanserization
			{
				PatientID = patientID,
				BeginDate = beginDate,
				EndDate = _fixture.Faker.Date.Soon(refDate: beginDate),
				IsClosed = false
			}))
			.Should().NotThrow<Exception>();

		FluentActions
			.Invoking(() => dispanserizationsRepository.Create(new Dispanserization
			{
				PatientID = patientID,
				BeginDate = beginDate,
				EndDate = _fixture.Faker.Date.Soon(refDate: beginDate),
				IsClosed = _fixture.Faker.Random.Bool()
			}))
			.Should().Throw<Exception>();
	}
}
