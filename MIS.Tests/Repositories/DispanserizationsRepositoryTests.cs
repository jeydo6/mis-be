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
		var beginDateTime = Faker.Date.Soon();

		// Act
		var host = CreateHost();
		var dispanserizationsRepository = host.Services.GetRequiredService<IDispanserizationsRepository>();
		var patientsRepository = host.Services.GetRequiredService<IPatientsRepository>();

		var dispanserizationResourcesIDs = CreateDispanserizationResources();
		CreateTimeItems(dispanserizationResourcesIDs, beginDateTime);

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
			BeginDate = beginDateTime.Date,
			EndDate = Faker.Date.Soon(refDate: beginDateTime.Date),
			IsClosed = Faker.Random.Bool()
		});

		// Assert
		var dispanserization = dispanserizationsRepository.Get(id);

		dispanserization.Should().NotBeNull();
		dispanserization.ID.Should().Be(id);
		dispanserization.Researches.Should().HaveSameCount(dispanserizationResourcesIDs);
	}

	[Fact]
	public void WhenCreate_WithToList_ThenReturnSuccess()
	{
		// Arrange
		var beginDateTime = Faker.Date.Soon();

		// Act
		var host = CreateHost();
		var dispanserizationsRepository = host.Services.GetRequiredService<IDispanserizationsRepository>();
		var patientsRepository = host.Services.GetRequiredService<IPatientsRepository>();

		var dispanserizationResourcesIDs = CreateDispanserizationResources();
		CreateTimeItems(dispanserizationResourcesIDs, beginDateTime);

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
			BeginDate = beginDateTime.Date,
			EndDate = Faker.Date.Soon(refDate: beginDateTime.Date),
			IsClosed = Faker.Random.Bool()
		});

		// Assert
		var dispanserizations = dispanserizationsRepository.ToList(patientID);

		dispanserizations.Should().NotBeNull();
		dispanserizations.Should().HaveCount(1);
		dispanserizations.Should().OnlyHaveUniqueItems();
		dispanserizations.Should().OnlyContain(d =>
			d.ID == id &&
			d.PatientID == patientID &&
			d.Researches.Count == dispanserizationResourcesIDs.Length
		);
	}

	[Fact]
	public void WhenCreate_WithDuplicate_ThenThrowException()
	{
		// Arrange
		var beginDateTime = Faker.Date.Soon();

		// Act/Assert
		var host = CreateHost();
		var dispanserizationsRepository = host.Services.GetRequiredService<IDispanserizationsRepository>();
		var patientsRepository = host.Services.GetRequiredService<IPatientsRepository>();

		var dispanserizationResourcesIDs = CreateDispanserizationResources();
		CreateTimeItems(dispanserizationResourcesIDs, beginDateTime);

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
				BeginDate = beginDateTime.Date,
				EndDate = Faker.Date.Soon(refDate: beginDateTime.Date),
				IsClosed = false
			}))
			.Should().NotThrow<Exception>();

		FluentActions
			.Invoking(() => dispanserizationsRepository.Create(new Dispanserization
			{
				PatientID = patientID,
				BeginDate = beginDateTime.Date,
				EndDate = Faker.Date.Soon(refDate: beginDateTime.Date),
				IsClosed = Faker.Random.Bool()
			}))
			.Should().Throw<Exception>();
	}
}
