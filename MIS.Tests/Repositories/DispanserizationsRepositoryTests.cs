using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MIS.Domain.Entities;
using MIS.Domain.Enums;
using MIS.Domain.Repositories;
using Xunit;

namespace MIS.Tests.Repositories;

[Collection("Database collection")]
public class DispanserizationsRepositoryTests : TestClassBase
{
	public DispanserizationsRepositoryTests(DatabaseFixture fixture) : base(fixture) { }

	[Fact]
	public void WhenCreate_WithGet_ThenReturnSuccess()
	{
		// Arrange
		var beginDate = Faker.Date.Soon().Date;

		// Act
		var host = CreateHost();
		var dispanserizationsRepository = host.Services.GetRequiredService<IDispanserizationsRepository>();
		var patientsRepository = host.Services.GetRequiredService<IPatientsRepository>();

		var dispanserizationResourcesIDs = CreateDispanserizationResources();
		CreateDispanserizationTimeItems(dispanserizationResourcesIDs);

		var patientID = patientsRepository.Create(new Patient
		{
			Code = Faker.Random.String2(8),
			BirthDate = Faker.Date.Past(30).Date,
			FirstName = Faker.Random.String2(10),
			MiddleName = Faker.Random.String2(10),
			LastName = Faker.Random.String2(10),
			Gender = Faker.PickRandomWithout(Gender.Unknown)
		});

		var id = dispanserizationsRepository.Create(new Dispanserization
		{
			PatientID = patientID,
			BeginDate = beginDate,
			EndDate = Faker.Date.Soon(refDate: beginDate),
			IsClosed = Faker.Random.Bool()
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
		var beginDate = Faker.Date.Soon().Date;

		// Act/Assert
		var host = CreateHost();
		var dispanserizationsRepository = host.Services.GetRequiredService<IDispanserizationsRepository>();
		var patientsRepository = host.Services.GetRequiredService<IPatientsRepository>();

		var dispanserizationResourcesIDs = CreateDispanserizationResources();
		CreateDispanserizationTimeItems(dispanserizationResourcesIDs);

		var patientID = patientsRepository.Create(new Patient
		{
			Code = Faker.Random.String2(8),
			BirthDate = Faker.Date.Past(30).Date,
			FirstName = Faker.Random.String2(10),
			MiddleName = Faker.Random.String2(10),
			LastName = Faker.Random.String2(10),
			Gender = Faker.PickRandomWithout(Gender.Unknown)
		});

		FluentActions
			.Invoking(() => dispanserizationsRepository.Create(new Dispanserization
			{
				PatientID = patientID,
				BeginDate = beginDate,
				EndDate = Faker.Date.Soon(refDate: beginDate),
				IsClosed = false
			}))
			.Should().NotThrow<Exception>();

		FluentActions
			.Invoking(() => dispanserizationsRepository.Create(new Dispanserization
			{
				PatientID = patientID,
				BeginDate = beginDate,
				EndDate = Faker.Date.Soon(refDate: beginDate),
				IsClosed = Faker.Random.Bool()
			}))
			.Should().Throw<Exception>();
	}
}
