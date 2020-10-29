using MediatR;
using MIS.Application.ViewModels;

namespace MIS.Application.Queries
{
    public class DispanserizationLastQuery : IRequest<DispanserizationViewModel>
    {
        public DispanserizationLastQuery(PatientViewModel patient)
        {
            Patient = patient;
        }

        public PatientViewModel Patient { get; }
    }
}
