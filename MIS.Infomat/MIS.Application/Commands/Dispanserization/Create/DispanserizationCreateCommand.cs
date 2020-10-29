using MediatR;
using MIS.Application.ViewModels;
using System;

namespace MIS.Application.Commands
{
    public class DispanserizationCreateCommand : IRequest<DispanserizationViewModel>
    {
        public DispanserizationCreateCommand(DateTime beginDate, Int32 patientID, String patientCode, String patientName)
        {
            BeginDate = beginDate;
            PatientID = patientID;
            PatientCode = patientCode;
            PatientName = patientName;
        }

        public DateTime BeginDate { get; }

        public Int32 PatientID { get; }

        public String PatientCode { get; }

        public String PatientName { get; }
    }
}
