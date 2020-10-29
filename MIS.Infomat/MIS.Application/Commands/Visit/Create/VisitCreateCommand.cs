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
