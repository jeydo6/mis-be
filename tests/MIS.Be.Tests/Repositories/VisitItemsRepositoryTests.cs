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
public class VisitItemsRepositoryTests : TestClassBase
{
	public VisitItemsRepositoryTests(TestApplicationFactory factory) : base(factory) { }

	[Fact]
	public void WhenCreate_WithGet_ThenReturnSuccess()
	{
		// Arrange
		var code = Faker.Random.String2(8);
		var birthDate = Faker.Date.Past(30).Date;
		var beginDateTime = Faker.Date.Soon();

		// Act
		var host = CreateHost();
		var visitItemsRepository = host.Services.GetRequiredService<IVisitItemsRepository>();
		var patientsRepository = host.Services.GetRequiredService<IPatientsRepository>();
		var timeItemsRepository = host.Services.GetRequiredService<ITimeItemsRepository>();
		var resourcesRepository = host.Services.GetRequiredService<IResourcesRepository>();
		var roomsRepository = host.Services.GetRequiredService<IRoomsRepository>();
		var employeesRepository = host.Services.GetRequiredService<IEmployeesRepository>();
		var specialtiesRepository = host.Services.GetRequiredService<ISpecialtiesRepository>();

		var patientID = patientsRepository.Create(new Patient
		{
			Code = code,
			BirthDate = birthDate,
			FirstName = Faker.Random.String2(10),
			MiddleName = Faker.Random.String2(10),
			LastName = Faker.Random.String2(10),
			Gender = Faker.PickRandomWithout(Gender.Unknown)
		});

		var specialtyID = specialtiesRepository.Create(new Specialty
		{
			Code = Faker.Random.String2(16),
			Name = Faker.Random.String2(10)
		});

		var employeeID = employeesRepository.Create(new Employee
		{
			Code = Faker.Random.String2(16),
			FirstName = Faker.Random.String2(10),
			MiddleName = Faker.Random.String2(10),
			LastName = Faker.Random.String2(10),
			SpecialtyID = specialtyID
		});

		var roomID = roomsRepository.Create(new Room
		{
			Code = Faker.Random.String2(16),
			Floor = Faker.Random.Int(1, 10)
		});

		var resourceID = resourcesRepository.Create(new Resource
		{
			EmployeeID = employeeID,
			RoomID = roomID,
			Type = ResourceType.Doctor,
			Name = Faker.Random.String2(10)
		});

		var timeItemID = timeItemsRepository.Create(new TimeItem
		{
			ResourceID = resourceID,
			Date = beginDateTime.Date,
			BeginDateTime = beginDateTime,
			EndDateTime = beginDateTime.AddMinutes(15),
		});

		var id = visitItemsRepository.Create(new VisitItem
		{
			PatientID = patientID,
			TimeItemID = timeItemID
		});

		// Assert
		var visitItem = visitItemsRepository.Get(id);

		visitItem.Should().NotBeNull();
		visitItem.ID.Should().Be(id);
		visitItem.Patient.Should().NotBeNull();
		visitItem.Patient.ID.Should().Be(patientID);
		visitItem.Patient.Code.Should().Be(code);
		visitItem.Patient.BirthDate.Should().BeSameDateAs(birthDate);
		visitItem.Patient.FirstName.Should().NotBeNull();
		visitItem.Patient.MiddleName.Should().NotBeNull();
		visitItem.Patient.LastName.Should().NotBeNullOrEmpty();
		visitItem.Patient.Gender.Should().BeDefined();
		visitItem.Patient.Gender.Should().NotBe(Gender.Unknown);
		visitItem.TimeItem.Should().NotBeNull();
		visitItem.TimeItem.ID.Should().Be(timeItemID);
		visitItem.TimeItem.Date.Should().Be(beginDateTime.Date);
		visitItem.TimeItem.BeginDateTime.Should().BeCloseTo(beginDateTime, TimeSpan.FromSeconds(1));
		visitItem.TimeItem.EndDateTime.Should().BeAfter(beginDateTime);
		visitItem.TimeItem.Resource.Should().NotBeNull();
		visitItem.TimeItem.Resource.ID.Should().Be(resourceID);
		visitItem.TimeItem.Resource.Name.Should().NotBeNull();
		visitItem.TimeItem.Resource.EmployeeID.Should().Be(employeeID);
		visitItem.TimeItem.Resource.Employee.Should().NotBeNull();
		visitItem.TimeItem.Resource.Employee.SpecialtyID.Should().Be(specialtyID);
		visitItem.TimeItem.Resource.Employee.Specialty.Should().NotBeNull();
		visitItem.TimeItem.Resource.Employee.Should().NotBeNull();
		visitItem.TimeItem.Resource.RoomID.Should().Be(roomID);
		visitItem.TimeItem.Resource.Room.Should().NotBeNull();
		visitItem.TimeItem.Resource.Type.Should().BeDefined();
		visitItem.TimeItem.Resource.Type.Should().NotBe(ResourceType.Unknown);
	}

	[Fact]
	public void WhenCreate_WithToList_ThenReturnSuccess()
	{
		// Arrange
		var code = Faker.Random.String2(8);
		var birthDate = Faker.Date.Past(30).Date;
		var beginDateTime = Faker.Date.Soon();

		// Act
		var host = CreateHost();
		var visitItemsRepository = host.Services.GetRequiredService<IVisitItemsRepository>();
		var patientsRepository = host.Services.GetRequiredService<IPatientsRepository>();
		var timeItemsRepository = host.Services.GetRequiredService<ITimeItemsRepository>();
		var resourcesRepository = host.Services.GetRequiredService<IResourcesRepository>();
		var roomsRepository = host.Services.GetRequiredService<IRoomsRepository>();
		var employeesRepository = host.Services.GetRequiredService<IEmployeesRepository>();
		var specialtiesRepository = host.Services.GetRequiredService<ISpecialtiesRepository>();

		var patientID = patientsRepository.Create(new Patient
		{
			Code = code,
			BirthDate = birthDate,
			FirstName = Faker.Random.String2(10),
			MiddleName = Faker.Random.String2(10),
			LastName = Faker.Random.String2(10),
			Gender = Faker.PickRandomWithout(Gender.Unknown)
		});

		var specialtyID = specialtiesRepository.Create(new Specialty
		{
			Code = Faker.Random.String2(16),
			Name = Faker.Random.String2(10)
		});

		var employeeID = employeesRepository.Create(new Employee
		{
			Code = Faker.Random.String2(16),
			FirstName = Faker.Random.String2(10),
			MiddleName = Faker.Random.String2(10),
			LastName = Faker.Random.String2(10),
			SpecialtyID = specialtyID
		});

		var roomID = roomsRepository.Create(new Room
		{
			Code = Faker.Random.String2(16),
			Floor = Faker.Random.Int(1, 10)
		});

		var resourceID = resourcesRepository.Create(new Resource
		{
			EmployeeID = employeeID,
			RoomID = roomID,
			Type = ResourceType.Doctor,
			Name = Faker.Random.String2(10)
		});

		var timeItemID = timeItemsRepository.Create(new TimeItem
		{
			ResourceID = resourceID,
			Date = beginDateTime.Date,
			BeginDateTime = beginDateTime,
			EndDateTime = beginDateTime.AddMinutes(15),
		});

		var id = visitItemsRepository.Create(new VisitItem
		{
			PatientID = patientID,
			TimeItemID = timeItemID
		});

		// Assert
		var visitItems = visitItemsRepository.ToList(beginDateTime.Date, beginDateTime.Date);

		visitItems.Should().NotBeNull();
		visitItems.Should().HaveCountGreaterThanOrEqualTo(1);
		visitItems.Should().OnlyHaveUniqueItems();
		visitItems.Should().OnlyContain(vi => vi.TimeItem != null && vi.TimeItem.Date == beginDateTime.Date);
		visitItems.Should().ContainSingle(vi =>
			vi.ID == id &&
			vi.PatientID == patientID &&
			vi.Patient == null &&
			vi.TimeItemID == timeItemID &&
			vi.TimeItem != null &&
			vi.TimeItem.ID == timeItemID &&
			vi.TimeItem.Date == beginDateTime.Date &&
			(vi.TimeItem.BeginDateTime - beginDateTime) < TimeSpan.FromSeconds(1) &&
			(vi.TimeItem.EndDateTime - beginDateTime.AddMinutes(15)) < TimeSpan.FromSeconds(1) &&
			vi.TimeItem.VisitItem != null &&
			vi.TimeItem.VisitItem.ID == id &&
			vi.TimeItem.Resource != null &&
			vi.TimeItem.Resource.Name != null &&
			vi.TimeItem.Resource.EmployeeID == employeeID &&
			vi.TimeItem.Resource.Employee != null &&
			vi.TimeItem.Resource.Employee.ID == employeeID &&
			vi.TimeItem.Resource.Employee.SpecialtyID == specialtyID &&
			vi.TimeItem.Resource.Employee.Specialty != null &&
			vi.TimeItem.Resource.Employee.Specialty.ID == specialtyID &&
			vi.TimeItem.Resource.RoomID == roomID &&
			vi.TimeItem.Resource.Room != null &&
			vi.TimeItem.Resource.Room.ID == roomID &&
			vi.TimeItem.Resource.Type > ResourceType.Unknown
		);
	}

	[Fact]
	public void WhenCreate_WithDuplicate_ThenThrowException()
	{
		// Arrange
		var birthDate = Faker.Date.Past(30).Date;
		var beginDateTime = Faker.Date.Soon();

		// Act/Assert
		var host = CreateHost();
		var visitItemsRepository = host.Services.GetRequiredService<IVisitItemsRepository>();
		var patientsRepository = host.Services.GetRequiredService<IPatientsRepository>();
		var timeItemsRepository = host.Services.GetRequiredService<ITimeItemsRepository>();
		var resourcesRepository = host.Services.GetRequiredService<IResourcesRepository>();
		var roomsRepository = host.Services.GetRequiredService<IRoomsRepository>();
		var employeesRepository = host.Services.GetRequiredService<IEmployeesRepository>();
		var specialtiesRepository = host.Services.GetRequiredService<ISpecialtiesRepository>();

		var patientID1 = patientsRepository.Create(new Patient
		{
			Code = Faker.Random.String2(8),
			BirthDate = birthDate,
			FirstName = Faker.Random.String2(10),
			MiddleName = Faker.Random.String2(10),
			LastName = Faker.Random.String2(10),
			Gender = Faker.PickRandomWithout(Gender.Unknown)
		});

		var patientID2 = patientsRepository.Create(new Patient
		{
			Code = Faker.Random.String2(8),
			BirthDate = birthDate,
			FirstName = Faker.Random.String2(10),
			MiddleName = Faker.Random.String2(10),
			LastName = Faker.Random.String2(10),
			Gender = Faker.PickRandomWithout(Gender.Unknown)
		});

		var specialtyID = specialtiesRepository.Create(new Specialty
		{
			Code = Faker.Random.String2(16),
			Name = Faker.Random.String2(10)
		});

		var employeeID = employeesRepository.Create(new Employee
		{
			Code = Faker.Random.String2(16),
			FirstName = Faker.Random.String2(10),
			MiddleName = Faker.Random.String2(10),
			LastName = Faker.Random.String2(10),
			SpecialtyID = specialtyID
		});

		var roomID = roomsRepository.Create(new Room
		{
			Code = Faker.Random.String2(16),
			Floor = Faker.Random.Int(1, 10)
		});

		var resourceID = resourcesRepository.Create(new Resource
		{
			EmployeeID = employeeID,
			RoomID = roomID,
			Type = ResourceType.Doctor,
			Name = Faker.Random.String2(10)
		});

		var timeItemID = timeItemsRepository.Create(new TimeItem
		{
			ResourceID = resourceID,
			Date = beginDateTime.Date,
			BeginDateTime = beginDateTime,
			EndDateTime = beginDateTime.AddMinutes(15),
		});

		FluentActions
			.Invoking(() => visitItemsRepository.Create(new VisitItem
			{
				PatientID = patientID1,
				TimeItemID = timeItemID
			}))
			.Should().NotThrow<Exception>();

		FluentActions
			.Invoking(() => visitItemsRepository.Create(new VisitItem
			{
				PatientID = patientID2,
				TimeItemID = timeItemID
			}))
			.Should().Throw<Exception>();
	}
}
