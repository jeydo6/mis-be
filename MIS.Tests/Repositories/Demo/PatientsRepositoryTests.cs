﻿using MIS.Demo.Repositories;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;
using MIS.Tests.Fixtures.Demo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MIS.Tests.Repositories.Demo
{
	public class PatientsRepositoryTests : IClassFixture<DataFixture>
	{
		private readonly IPatientsRepository _patients;

		public PatientsRepositoryTests(DataFixture dataFixture)
		{
			_patients = new PatientsRepository(dataFixture.DateTimeProvider, dataFixture.DataContext);
		}

		[Fact]
		public async Task First_Ok()
		{
			var actualResult = await _patients.First("30000000", new DateTime(1980, 1, 1));
			var expectedResult = new Patient
			{
				Code = "30000000",
				Name = "Иван Иванович",
				BirthDate = new DateTime(1980, 1, 1),
				GenderID = 0,
				Dispanserizations = new List<Dispanserization>(),
				VisitItems = new List<VisitItem>()
			};

			Assert.Equal(
				new
				{
					expectedResult.Code,
					expectedResult.Name,
					expectedResult.BirthDate,
					expectedResult.GenderID
				},
				new
				{
					actualResult.Code,
					actualResult.Name,
					actualResult.BirthDate,
					actualResult.GenderID
				}
			);
		}

		[Fact]
		public async Task Get_Ok()
		{
			var patient = await _patients.First("30000000", new DateTime(1980, 1, 1));

			var actualResult = await _patients.Get(patient.ID);
			var expectedResult = new Patient
			{
				ID = actualResult.ID,
				Code = "30000000",
				Name = "Иван Иванович",
				BirthDate = new DateTime(1980, 1, 1),
				GenderID = 0,
				Dispanserizations = new List<Dispanserization>(),
				VisitItems = new List<VisitItem>()
			};

			Assert.Equal(
				new
				{
					patient.ID,
					expectedResult.Code,
					expectedResult.Name,
					expectedResult.BirthDate,
					expectedResult.GenderID
				},
				new
				{
					actualResult.ID,
					actualResult.Code,
					actualResult.Name,
					actualResult.BirthDate,
					actualResult.GenderID
				}
			);
		}
	}
}
