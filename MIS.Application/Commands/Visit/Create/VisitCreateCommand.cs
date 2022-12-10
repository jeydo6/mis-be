﻿using MIS.Application.ViewModels;
using MIS.Mediator;

namespace MIS.Application.Commands
{
	public class VisitCreateCommand : IRequest<VisitItemViewModel>
	{
		public VisitCreateCommand(int timeItemID, int patientID, string patientCode, string patientName)
		{
			TimeItemID = timeItemID;
			PatientID = patientID;
			PatientCode = patientCode;
			PatientName = patientName;
		}

		public int TimeItemID { get; }

		public int PatientID { get; }

		public string PatientCode { get; }

		public string PatientName { get; }
	}
}
