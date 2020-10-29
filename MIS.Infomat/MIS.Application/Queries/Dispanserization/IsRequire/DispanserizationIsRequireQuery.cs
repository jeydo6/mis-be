using MediatR;
using MIS.Application.ViewModels;
using System;

namespace MIS.Application.Queries
{
    public class DispanserizationIsRequireQuery : IRequest<Boolean>
    {
        public DispanserizationIsRequireQuery(PatientViewModel patient)
        {
            Patient = patient;
        }

        public PatientViewModel Patient { get; }
    }
}
