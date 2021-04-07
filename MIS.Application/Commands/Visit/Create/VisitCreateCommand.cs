#region Copyright © 2020-2021 Vladimir Deryagin. All rights reserved
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

using MediatR;
using MIS.Application.ViewModels;
using System;

namespace MIS.Application.Commands
{
	public class VisitCreateCommand : IRequest<VisitItemViewModel>
	{
		public VisitCreateCommand(Int32 timeItemID, Int32 patientID, String patientCode, String patientName)
		{
			TimeItemID = timeItemID;
			PatientID = patientID;
			PatientCode = patientCode;
			PatientName = patientName;
		}

		public Int32 TimeItemID { get; }

		public Int32 PatientID { get; }

		public String PatientCode { get; }

		public String PatientName { get; }
	}
}
