using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Enums;
using MIS.Be.Domain.Repositories;
using MIS.Be.Tests.Factories;
using Xunit;

namespace MIS.Be.Tests.Repositories;

[Collection("Database collection")]
public class TimeItemsRepositoryTests : TestClassBase
{
	public TimeItemsRepositoryTests(TestApplicationFactory factory) : base(factory) { }

	[Fact]
	public void WhenCreate_WithGet_ThenReturnSuccess()
	{
		// Arrange
		var beginDateTime = Faker.Date.Soon();

		// Act
		var host = CreateHost();
		var timeItemsRepository = host.Services.GetRequiredService<ITimeItemsRepository>();
		var resourcesRepository = host.Services.GetRequiredService<IResourcesRepository>();
		var roomsRepository = host.Services.GetRequiredService<IRoomsRepository>();
		var employeesRepository = host.Services.GetRequiredService<IEmployeesRepository>();
		var specialtiesRepository = host.Services.GetRequiredService<ISpecialtiesRepository>();

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

		var id = timeItemsRepository.Create(new TimeItem
		{
			ResourceID = resourceID,
			Date = beginDateTime.Date,
			BeginDateTime = beginDateTime,
			EndDateTime = beginDateTime.AddMinutes(15),
		});

		// Assert
		var timeItem = timeItemsRepository.Get(id);

		timeItem.Should().NotBeNull();
		timeItem.ID.Should().Be(id);
		timeItem.Date.Should().Be(beginDateTime.Date);
		timeItem.BeginDateTime.Should().BeCloseTo(beginDateTime, TimeSpan.FromSeconds(1));
		timeItem.EndDateTime.Should().BeAfter(beginDateTime);
		timeItem.VisitItem.Should().BeNull();
		timeItem.Resource.Should().NotBeNull();
		timeItem.Resource.ID.Should().Be(resourceID);
		timeItem.Resource.Name.Should().NotBeNull();
		timeItem.Resource.EmployeeID.Should().Be(employeeID);
		timeItem.Resource.Employee.Should().NotBeNull();
		timeItem.Resource.Employee.SpecialtyID.Should().Be(specialtyID);
		timeItem.Resource.Employee.Specialty.Should().NotBeNull();
		timeItem.Resource.Employee.Should().NotBeNull();
		timeItem.Resource.RoomID.Should().Be(roomID);
		timeItem.Resource.Room.Should().NotBeNull();
		timeItem.Resource.Type.Should().BeDefined();
		timeItem.Resource.Type.Should().NotBe(ResourceType.Unknown);
	}

	[Fact]
	public void WhenCreate_WithToList_ThenReturnSuccess()
	{
		// Arrange
		var beginDateTime = Faker.Date.Soon();

		// Act
		var host = CreateHost();
		var timeItemsRepository = host.Services.GetRequiredService<ITimeItemsRepository>();
		var resourcesRepository = host.Services.GetRequiredService<IResourcesRepository>();
		var roomsRepository = host.Services.GetRequiredService<IRoomsRepository>();
		var employeesRepository = host.Services.GetRequiredService<IEmployeesRepository>();
		var specialtiesRepository = host.Services.GetRequiredService<ISpecialtiesRepository>();

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

		var id = timeItemsRepository.Create(new TimeItem
		{
			ResourceID = resourceID,
			Date = beginDateTime.Date,
			BeginDateTime = beginDateTime,
			EndDateTime = beginDateTime.AddMinutes(15),
		});

		// Assert
		var timeItems = timeItemsRepository.ToList(beginDateTime.Date, beginDateTime.Date);

		timeItems.Should().NotBeNull();
		timeItems.Should().HaveCountGreaterThanOrEqualTo(1);
		timeItems.Should().OnlyHaveUniqueItems();
		timeItems.Should().OnlyContain(ti => ti.Date == beginDateTime.Date);
		timeItems.Should().ContainSingle(ti =>
			ti.ID == id &&
			ti.Date == beginDateTime.Date &&
			(ti.BeginDateTime - beginDateTime) < TimeSpan.FromSeconds(1) &&
			(ti.EndDateTime - beginDateTime.AddMinutes(15)) < TimeSpan.FromSeconds(1) &&
			ti.VisitItem == null &&
			ti.Resource != null &&
			ti.Resource.Name != null &&
			ti.Resource.EmployeeID == employeeID &&
			ti.Resource.Employee != null &&
			ti.Resource.Employee.ID == employeeID &&
			ti.Resource.Employee.SpecialtyID == specialtyID &&
			ti.Resource.Employee.Specialty != null &&
			ti.Resource.Employee.Specialty.ID == specialtyID &&
			ti.Resource.RoomID == roomID &&
			ti.Resource.Room != null &&
			ti.Resource.Room.ID == roomID &&
			ti.Resource.Type > ResourceType.Unknown
		);
	}

	[Fact]
	public void WhenCreate_WithGetResourceTotals_ThenReturnSuccess()
	{
		// Arrange
		var beginDateTime = Faker.Date.Soon();

		// Act
		var host = CreateHost();
		var timeItemsRepository = host.Services.GetRequiredService<ITimeItemsRepository>();
		var resourcesRepository = host.Services.GetRequiredService<IResourcesRepository>();
		var roomsRepository = host.Services.GetRequiredService<IRoomsRepository>();
		var employeesRepository = host.Services.GetRequiredService<IEmployeesRepository>();
		var specialtiesRepository = host.Services.GetRequiredService<ISpecialtiesRepository>();

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

		var id = timeItemsRepository.Create(new TimeItem
		{
			ResourceID = resourceID,
			Date = beginDateTime.Date,
			BeginDateTime = beginDateTime,
			EndDateTime = beginDateTime.AddMinutes(15),
		});

		// Assert
		var timeItemTotals = timeItemsRepository.GetResourceTotals(beginDateTime.Date, beginDateTime.Date);

		timeItemTotals.Should().NotBeNull();
		timeItemTotals.Should().HaveCountGreaterThanOrEqualTo(1);
		timeItemTotals.Should().OnlyHaveUniqueItems();
		timeItemTotals.Should().OnlyContain(t => t.Date == beginDateTime.Date);
		timeItemTotals.Should().ContainSingle(t =>
			t.Date == beginDateTime.Date &&
			(t.BeginDateTime - beginDateTime) < TimeSpan.FromSeconds(1) &&
			(t.EndDateTime - beginDateTime.AddMinutes(15)) < TimeSpan.FromSeconds(1) &&
			t.ResourceID == resourceID &&
			t.TimesCount == 1 &&
			t.VisitsCount == 0
		);
	}

	[Fact]
	public void WhenCreate_WithGetDispanserizationTotals_ThenReturnSuccess()
	{
		// Arrange
		var beginDateTime = Faker.Date.Soon();

		// Act
		var host = CreateHost();
		var timeItemsRepository = host.Services.GetRequiredService<ITimeItemsRepository>();
		var resourcesRepository = host.Services.GetRequiredService<IResourcesRepository>();
		var roomsRepository = host.Services.GetRequiredService<IRoomsRepository>();
		var employeesRepository = host.Services.GetRequiredService<IEmployeesRepository>();
		var specialtiesRepository = host.Services.GetRequiredService<ISpecialtiesRepository>();

		var dispanserizationResourcesIDs = CreateDispanserizationResources();
		CreateTimeItems(dispanserizationResourcesIDs, beginDateTime);

		// Assert
		var timeItemTotals = timeItemsRepository.GetDispanserizationTotals(beginDateTime.Date, beginDateTime.Date);

		timeItemTotals.Should().NotBeNull();
		timeItemTotals.Should().HaveCount(2);
		timeItemTotals.Should().OnlyHaveUniqueItems();
		timeItemTotals.Should().OnlyContain(t =>
			t.Date == beginDateTime.Date &&
			t.EndDateTime > t.BeginDateTime &&
			t.TimesCount > t.VisitsCount &&
			dispanserizationResourcesIDs.Contains(t.ResourceID)
		);
	}

	[Fact]
	public void WhenCreate_WithDuplicate_ThenThrowException()
	{
		// Arrange
		var beginDateTime = Faker.Date.Soon();

		// Act/Assert
		var host = CreateHost();
		var timeItemsRepository = host.Services.GetRequiredService<ITimeItemsRepository>();
		var resourcesRepository = host.Services.GetRequiredService<IResourcesRepository>();
		var roomsRepository = host.Services.GetRequiredService<IRoomsRepository>();
		var employeesRepository = host.Services.GetRequiredService<IEmployeesRepository>();
		var specialtiesRepository = host.Services.GetRequiredService<ISpecialtiesRepository>();

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

		FluentActions
			.Invoking(() => timeItemsRepository.Create(new TimeItem
			{
				ResourceID = resourceID,
				Date = beginDateTime.Date,
				BeginDateTime = beginDateTime,
				EndDateTime = beginDateTime.AddMinutes(15),
			}))
			.Should().NotThrow<Exception>();

		FluentActions
			.Invoking(() => timeItemsRepository.Create(new TimeItem
			{
				ResourceID = resourceID,
				Date = beginDateTime.Date,
				BeginDateTime = beginDateTime,
				EndDateTime = beginDateTime.AddMinutes(15),
			}))
			.Should().Throw<Exception>();
	}
}
