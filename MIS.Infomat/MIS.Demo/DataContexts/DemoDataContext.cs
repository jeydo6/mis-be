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
                    FirstName = "Иван",
                    MiddleName = "Иванович",
                    BirthDate = new DateTime(1980, 1, 1),
                    Gender = 0,
                    Dispanserizations = new List<Dispanserization>(),
                    VisitItems = new List<VisitItem>()
                },
                new Patient
                {
                    ID = 2,
                    Code = "31000000",
                    FirstName = "Михаил",
                    MiddleName = "Михайлович",
                    BirthDate = new DateTime(1980, 1, 1),
                    Gender = 0,
                    Dispanserizations = new List<Dispanserization>(),
                    VisitItems = new List<VisitItem>()
                },
                new Patient
                {
                    ID = Int32.MaxValue,
                    Code = "32020444",
                    FirstName = "Владимир",
                    MiddleName = "Сергеевич",
                    BirthDate = new DateTime(1993, 1, 1),
                    Gender = 0,
                    Dispanserizations = new List<Dispanserization>(),
                    VisitItems = new List<VisitItem>()
                }
            };
            Resources = new List<Resource>
            {
                new Resource
                {
                    ID = 1,
                    DoctorID = 1,
                    Doctor = new Doctor
                    {
                        ID = 1,
                        Code = "100",
                        FirstName = "Пётр",
                        MiddleName = "Петрович",
                        LastName = "Петров",
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
                        Code = "1",
                        Flat = 1
                    }
                },
                new Resource
                {
                    ID = 2,
                    DoctorID = 2,
                    Doctor = new Doctor
                    {
                        ID = 2,
                        Code = "200",
                        FirstName = "Сергей",
                        MiddleName = "Владимирович",
                        LastName = "Кузнецов",
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
                    DoctorID = 3,
                    Doctor = new Doctor
                    {
                        ID = 3,
                        Code = "300",
                        FirstName = "Михаил",
                        MiddleName = "Семёнович",
                        LastName = "Костоломов",
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
                    DoctorID = 4,
                    Doctor = new Doctor
                    {
                        ID = 4,
                        Code = "400",
                        FirstName = "Кирилл",
                        MiddleName = "Александрович",
                        LastName = "Яковлев",
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
                    DoctorID = 5,
                    Doctor = new Doctor
                    {
                        ID = 5,
                        Code = "500",
                        FirstName = "Анастасия",
                        MiddleName = "Яковлевна",
                        LastName = "Пономаренко",
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
                    DoctorID = 6,
                    Doctor = new Doctor
                    {
                        ID = 6,
                        Code = "600",
                        FirstName = "Яков",
                        MiddleName = "Анастархович",
                        LastName = "Вахроменко",
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
