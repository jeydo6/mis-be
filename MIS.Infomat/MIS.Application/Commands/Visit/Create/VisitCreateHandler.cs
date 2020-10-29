using MediatR;
using MIS.Application.ViewModels;
using MIS.Domain.Entities;
using MIS.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Commands
{
    public class VisitCreateHandler : IRequestHandler<VisitCreateCommand, VisitItemViewModel>
    {
        private readonly IVisitItemsRepository _visitItems;

        public VisitCreateHandler(
            IVisitItemsRepository visitItems
        )
        {
            _visitItems = visitItems;
        }

        public async Task<VisitItemViewModel> Handle(VisitCreateCommand request, CancellationToken cancellationToken)
        {
            VisitItem visitItem = new VisitItem
            {
                TimeItemID = request.TimeItemID,
                PatientID = request.PatientID
            };

            Int32 visitItemID = _visitItems.Create(visitItem);

            visitItem = _visitItems.Get(visitItemID);

            VisitItemViewModel viewModel = new VisitItemViewModel
            {
                BeginDateTime = visitItem.TimeItem.BeginDateTime,
                PatientCode = request.PatientCode,
                PatientName = request.PatientName,
                DoctorName = visitItem.TimeItem.Resource.Doctor.DisplayName,
                SpecialtyName = visitItem.TimeItem.Resource.Doctor.Specialty.Name,
                RoomCode = visitItem.TimeItem.Resource.Room.Code,
                RoomFlat = visitItem.TimeItem.Resource.Room.Flat,
                IsEnabled = true,
                ResourceID = visitItem.TimeItem.ResourceID
            };

            return await Task.FromResult(viewModel);
        }
    }
}
