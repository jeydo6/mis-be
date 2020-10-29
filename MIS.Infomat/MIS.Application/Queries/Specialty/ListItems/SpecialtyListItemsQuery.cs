using MediatR;
using MIS.Application.ViewModels;
using System.Collections.Generic;

namespace MIS.Application.Queries
{
    public class SpecialtyListItemsQuery : IRequest<IEnumerable<SpecialtyViewModel>>
    {
        public SpecialtyListItemsQuery(PatientViewModel patient)
        {
            Patient = patient;
        }

        public PatientViewModel Patient { get; }
    }
}
