﻿using MIS.Be.Application.ViewModels;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Extensions;
using MIS.Be.Domain.Repositories;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Commands
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

		public VisitItemViewModel Handle(VisitCreateCommand request)
		{
			var visitItem = new VisitItem
			{
				TimeItemID = request.TimeItemID,
				PatientID = request.PatientID
			};

			var visitItemID = _visitItems.Create(visitItem);

			visitItem = _visitItems.Get(visitItemID);

			var result = new VisitItemViewModel
			{
				BeginDateTime = visitItem.TimeItem.BeginDateTime,
				PatientCode = request.PatientCode,
				PatientName = request.PatientName,
				ResourceName = visitItem.TimeItem.Resource.Name,
				EmployeeName = visitItem.TimeItem.Resource.Employee.GetName(),
				SpecialtyName = visitItem.TimeItem.Resource.Employee.Specialty.Name,
				RoomCode = visitItem.TimeItem.Resource.Room.Code,
				RoomFloor = visitItem.TimeItem.Resource.Room.Floor,
				IsEnabled = true,
				ResourceID = visitItem.TimeItem.ResourceID
			};

			return result;
		}
	}
}
