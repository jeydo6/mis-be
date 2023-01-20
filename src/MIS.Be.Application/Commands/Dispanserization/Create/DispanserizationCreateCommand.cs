using System;
using MIS.Be.Application.ViewModels;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Commands
{
	public class DispanserizationCreateCommand : IRequest<DispanserizationViewModel>
	{
		public DispanserizationCreateCommand(DateTime beginDate, int patientID, string patientCode, string patientName)
		{
			BeginDate = beginDate;
			PatientID = patientID;
			PatientCode = patientCode;
			PatientName = patientName;
		}

		public DateTime BeginDate { get; }

		public int PatientID { get; }

		public string PatientCode { get; }

		public string PatientName { get; }
	}
}
