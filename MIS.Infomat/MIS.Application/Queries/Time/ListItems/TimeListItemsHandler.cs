using MediatR;
using MIS.Application.ViewModels;
using MIS.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Queries
{
    public class TimeListItemsHandler : IRequestHandler<TimeListItemsQuery, IEnumerable<TimeItemViewModel>>
    {
        private readonly ITimeItemsRepository _timeItems;

        public TimeListItemsHandler(
            ITimeItemsRepository timeItems
        )
        {
            _timeItems = timeItems;
        }

        public async Task<IEnumerable<TimeItemViewModel>> Handle(TimeListItemsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<TimeItemViewModel> viewModels = _timeItems
                .ToList(request.Date, request.Date, request.ResourceID)
                .Select(t => new TimeItemViewModel
                {
                    TimeItemID = t.ID,
                    DateTime = t.BeginDateTime,
                    IsEnabled = t.VisitItem == null
                })
                .OrderBy(ti => ti.DateTime)
                .Take(28)
                .ToList();

            return await Task.FromResult(viewModels);
        }
    }
}
