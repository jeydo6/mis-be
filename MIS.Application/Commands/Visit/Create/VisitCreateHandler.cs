﻿#region Copyright © 2018-2022 Vladimir Deryagin. All rights reserved
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

using MIS.Application.ViewModels;
using MIS.Domain.Entities;
using MIS.Domain.Extensions;
using MIS.Domain.Repositories;
using MIS.Mediator;

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
