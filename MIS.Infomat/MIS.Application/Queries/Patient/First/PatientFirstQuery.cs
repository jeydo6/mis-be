using MediatR;
using MIS.Application.ViewModels;
using System;

namespace MIS.Application.Queries
{
    public class PatientFirstQuery : IRequest<PatientViewModel>
    {
        public PatientFirstQuery(String code, DateTime birthDate)
        {
            Code = code;
            BirthDate = birthDate;
        }

        public String Code { get; }

        public DateTime BirthDate { get; }
    }
}
