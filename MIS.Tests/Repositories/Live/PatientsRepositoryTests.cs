using System;
using System.Collections.Generic;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;
using MIS.Persistence.Repositories;
using MIS.Tests.Fixtures.Live;
using Xunit;

namespace MIS.Tests.Repositories.Live
{
	public class PatientsRepositoryTests : IClassFixture<DataFixture>
	{
		private readonly IPatientsRepository _patients;

		public PatientsRepositoryTests(DataFixture dataFixture)
		{
			_patients = new PatientsRepository(dataFixture.ConnectionString);
		}

		[Fact]
		public void First_Ok()
		{
			var actualResult = _patients.First("30000000", new DateTime(1980, 1, 1));
			var expectedResult = new Patient
			{
				Code = "30000000",
				FirstName = "Иван",
				MiddleName = "Иванович",
				LastName = "Иванов",
				BirthDate = new DateTime(1980, 1, 1),
				GenderID = 0,
				Dispanserizations = new List<Dispanserization>(),
				VisitItems = new List<VisitItem>()
			};

			Assert.Equal(
				new
				{
					expectedResult.Code,
					expectedResult.FirstName,
					expectedResult.MiddleName,
					expectedResult.LastName,
					expectedResult.BirthDate,
					expectedResult.GenderID
				},
				new
				{
					actualResult.Code,
					actualResult.FirstName,
					actualResult.MiddleName,
					actualResult.LastName,
					actualResult.BirthDate,
					actualResult.GenderID
				}
			);
		}

		[Fact]
		public void Get_Ok()
		{
			var patient = _patients.First("30000000", new DateTime(1980, 1, 1));

			var actualResult = _patients.Get(patient.ID);
			var expectedResult = new Patient
			{
				ID = actualResult.ID,
				Code = "30000000",
				FirstName = "Иван",
				MiddleName = "Иванович",
				LastName = "Иванов",
				BirthDate = new DateTime(1980, 1, 1),
				GenderID = 0,
				Dispanserizations = new List<Dispanserization>(),
				VisitItems = new List<VisitItem>()
			};

			Assert.Equal(
				new
				{
					expectedResult.Code,
					expectedResult.FirstName,
					expectedResult.MiddleName,
					expectedResult.LastName,
					expectedResult.BirthDate,
					expectedResult.GenderID
				},
				new
				{
					actualResult.Code,
					actualResult.FirstName,
					actualResult.MiddleName,
					actualResult.LastName,
					actualResult.BirthDate,
					actualResult.GenderID
				}
			);
		}
	}
}
