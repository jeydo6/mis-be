using MediatR;
using MIS.Application.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Queries
{
    public class VisitListItemsHandler : IRequestHandler<VisitListItemsQuery, IEnumerable<VisitItemViewModel>>
    {
        public async Task<IEnumerable<VisitItemViewModel>> Handle(VisitListItemsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<VisitItemViewModel> visitItems = request.Patient.VisitItems
                .OrderBy(v => v.BeginDateTime)
                .ToList();

            return await Task.FromResult(visitItems);
        }
    }
}
