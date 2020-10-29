using MediatR;
using MIS.Application.ViewModels;
using System.Collections.Generic;

namespace MIS.Application.Queries
{
    public class VisitListItemsQuery : IRequest<IEnumerable<VisitItemViewModel>>
    {
        public VisitListItemsQuery(PatientViewModel patient)
        {
            Patient = patient;
        }

        public PatientViewModel Patient { get; }
    }
}
