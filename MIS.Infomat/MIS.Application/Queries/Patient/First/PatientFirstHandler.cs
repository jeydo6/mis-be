﻿using MediatR;
using MIS.Application.ViewModels;
using MIS.Domain.Entities;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Queries
{
    public class PatientFirstHandler : IRequestHandler<PatientFirstQuery, PatientViewModel>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IPatientsRepository _patients;
        private readonly IVisitItemsRepository _visitItems;
        private readonly IDispanserizationsRepository _dispanserizations;

        public PatientFirstHandler(
            IDateTimeProvider dateTimeProvider,
            IPatientsRepository patients,
            IVisitItemsRepository visitItems,
            IDispanserizationsRepository dispanserizations
        )
        {
            _dateTimeProvider = dateTimeProvider;
            _patients = patients;
            _visitItems = visitItems;
            _dispanserizations = dispanserizations;
        }

        public async Task<PatientViewModel> Handle(PatientFirstQuery request, CancellationToken cancellationToken)
        {
            Patient patient = _patients.First(request.Code, request.BirthDate);

            if (patient == null)
            {
                return null;
            }

            DateTime beginDate = _dateTimeProvider.Now.Date;
            DateTime endDate = _dateTimeProvider.Now.Date.AddDays(28);

            patient.VisitItems = _visitItems
                .ToList(beginDate, endDate, patientID: patient.ID)
                .ToList();

            patient.Dispanserizations = _dispanserizations
                .ToList(patient.ID)
                .ToList();

            PatientViewModel viewModel = new PatientViewModel
            {
                ID = patient.ID,
                Code = patient.Code,
                FirstName = patient.FirstName,
                MiddleName = patient.MiddleName,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                Dispanserizations = patient.Dispanserizations.Select(d => new DispanserizationViewModel
                {
                    BeginDate = d.BeginDate,
                    Today = _dateTimeProvider.Now.Date,
                    PatientCode = patient.Code,
                    PatientName = patient.DisplayName,
                    IsClosed = d.IsClosed,
                    IsEnabled = true,
                    Analyses = d.Analyses.Select(a => a.Description).ToList()
                }).ToList(),
                VisitItems = patient.VisitItems.Select(vi => new VisitItemViewModel
                {
                    BeginDateTime = vi.TimeItem.BeginDateTime,
                    PatientCode = patient.Code,
                    PatientName = patient.DisplayName,
                    DoctorName = vi.TimeItem.Resource.Doctor.DisplayName,
                    SpecialtyName = vi.TimeItem.Resource.Doctor.Specialty.Name,
                    RoomCode = vi.TimeItem.Resource.Room.Code,
                    RoomFlat = vi.TimeItem.Resource.Room.Flat,
                    IsEnabled = true,
                    ResourceID = vi.TimeItem.ResourceID
                }).ToList()
            };

            return await Task.FromResult(viewModel);
        }
    }
}
