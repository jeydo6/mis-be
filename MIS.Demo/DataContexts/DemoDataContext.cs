#region Copyright © 2018-2022 Vladimir Deryagin. All rights reserved
/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

using MIS.Domain.Entities;
using MIS.Domain.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MIS.Demo.DataContexts
{
	public class DemoDataContext
	{
		private readonly IDateTimeProvider _dateTimeProvider;

		public DemoDataContext(IDateTimeProvider dateTimeProvider)
		{
			_dateTimeProvider = dateTimeProvider;

			Dispanserizations = new List<Dispanserization>();
			Patients = new List<Patient>
			{
				new Patient
				{
					ID = 1,
					Code = "30000000",
					Name = "Иван Иванович",
					BirthDate = new DateTime(1980, 1, 1),
					GenderID = 0,
					DispanserizationIsEnabled = true,
					Dispanserizations = new List<Dispanserization>(),
					VisitItems = new List<VisitItem>()
				},
				new Patient
				{
					ID = 2,
					Code = "31000000",
					Name = "Михаил Михайлович",
					BirthDate = new DateTime(1980, 1, 1),
					GenderID = 0,
					DispanserizationIsEnabled = true,
					Dispanserizations = new List<Dispanserization>(),
					VisitItems = new List<VisitItem>()
				},
				new Patient
				{
					ID = Int32.MaxValue,
					Code = "32020444",
					Name = "Владимир Сергеевич",
					BirthDate = new DateTime(1993, 1, 1),
					GenderID = 0,
					DispanserizationIsEnabled = true,
					Dispanserizations = new List<Dispanserization>(),
					VisitItems = new List<VisitItem>()
				}
			};
			Resources = new List<Resource>
			{
				new Resource
				{
					ID = 1,
					Name = "Врач-терапевт",
					EmployeeID = 1,
					Employee = new Employee
					{
						ID = 1,
						Code = "100",
						Name = "Петров П. П.",
						SpecialtyID = 1,
						Specialty = new Specialty
						{
							ID = 1,
							Code = "1",
							Name = "Терапия"
						}
					},
					RoomID = 1,
					Room = new Room
					{
						ID = 1,
						Code = "100",
						Flat = 1
					}
				},
				new Resource
				{
					ID = 2,
					Name = "Врач-терапевт",
					EmployeeID = 2,
					Employee = new Employee
					{
						ID = 2,
						Code = "200",
						Name = "Кузнецов С. В.",
						SpecialtyID = 1,
						Specialty = new Specialty
						{
							ID = 1,
							Code = "1",
							Name = "Терапия"
						}
					},
					RoomID = 2,
					Room = new Room
					{
						ID = 2,
						Code = "200",
						Flat = 2
					}
				},
				new Resource
				{
					ID = 3,
					Name = "Врач-хирург",
					EmployeeID = 3,
					Employee = new Employee
					{
						ID = 3,
						Code = "300",
						Name = "Костоломов М. С.",
						SpecialtyID = 2,
						Specialty = new Specialty
						{
							ID = 2,
							Code = "2",
							Name = "Хирургия"
						}
					},
					RoomID = 3,
					Room = new Room
					{
						ID = 3,
						Code = "300",
						Flat = 3
					}
				},
				new Resource
				{
					ID = 4,
					Name = "Врач диспансерного отделения",
					EmployeeID = 4,
					Employee = new Employee
					{
						ID = 4,
						Code = "400",
						Name = "Яковлев К. А.",
						SpecialtyID = 3,
						Specialty = new Specialty
						{
							ID = 3,
							Code = "3",
							Name = "Диспансеризация"
						}
					},
					RoomID = 4,
					Room = new Room
					{
						ID = 4,
						Code = "400",
						Flat = 4
					}
				},
				new Resource
				{
					ID = 5,
					Name = "Не определено",
					EmployeeID = 5,
					Employee = new Employee
					{
						ID = 5,
						Code = "500",
						Name = "Пономаренко А. Я.",
						SpecialtyID = 0,
						Specialty = new Specialty
						{
							ID = 0,
							Code = "0",
							Name = "Не определено"
						}
					},
					RoomID = 500,
					Room = new Room
					{
						ID = 500,
						Code = "500",
						Flat = 5
					}
				},
				new Resource
				{
					ID = 6,
					Name = "Не определено",
					EmployeeID = 6,
					Employee = new Employee
					{
						ID = 6,
						Code = "600",
						Name = "Вахроменко Я. А.",
						SpecialtyID = 0,
						Specialty = new Specialty
						{
							ID = 0,
							Code = "0",
							Name = "Не определено"
						}
					},
					RoomID = 6,
					Room = new Room
					{
						ID = 6,
						Code = "600",
						Flat = 6
					}
				}
			};
			TimeItems = new List<TimeItem>();
			VisitItems = new List<VisitItem>();

			Int32 timeItemID = 0;
			foreach (Resource resource in Resources)
			{
				for (Int32 i = 0; i < 28; i++)
				{
					for (Int32 j = 0; j < 24; j++)
					{
						TimeItems.Add(new TimeItem
						{
							ID = ++timeItemID,
							Date = _dateTimeProvider.Now.Date.AddDays(i),
							BeginDateTime = _dateTimeProvider.Now.Date.AddDays(i).AddHours(8).AddMinutes(j * 15),
							EndDateTime = _dateTimeProvider.Now.Date.AddDays(i).AddHours(8).AddMinutes(j * 15 + 15),
							ResourceID = resource.ID,
							Resource = resource
						});
					}
				}
			}

			VisitItem visitItem = new VisitItem
			{
				ID = 1,
				PatientID = 1,
				Patient = Patients.FirstOrDefault(p => p.ID == 1),
				TimeItemID = 1,
				TimeItem = TimeItems.FirstOrDefault(ti => ti.ID == 1)
			};
			visitItem.TimeItem.VisitItem = visitItem;
			visitItem.Patient.VisitItems.Add(visitItem);
			VisitItems.Add(visitItem);
		}

		internal List<Dispanserization> Dispanserizations { get; set; }

		internal List<Patient> Patients { get; set; }

		internal List<Resource> Resources { get; set; }

		internal List<TimeItem> TimeItems { get; set; }

		internal List<VisitItem> VisitItems { get; set; }
	}
}
