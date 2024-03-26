using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Filters;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetResourceDateItemsHandler : IRequestHandler<GetResourceDateItemsQuery, DateItem[]>
{
    private readonly ITimeItemsRepository _timeItemsRepository;
    private readonly IVisitItemsRepository _visitItemsRepository;

    public GetResourceDateItemsHandler(
        ITimeItemsRepository timeItemsRepository,
        IVisitItemsRepository visitItemsRepository)
    {
        _timeItemsRepository = timeItemsRepository;
        _visitItemsRepository = visitItemsRepository;
    }

    public async Task<DateItem[]> Handle(GetResourceDateItemsQuery request, CancellationToken cancellationToken)
    {
        var visitItems = await _visitItemsRepository.GetAll(request.From, request.To,
            filter: new GetAllVisitItemsFilter(ResourceId: request.ResourceId),
            cancellationToken: cancellationToken);
        var timeItemIds = visitItems.Select(vi => vi.TimeItemId).ToHashSet();

        var timeItems = await _timeItemsRepository.GetAll(request.From, request.To,
            filter: new GetAllTimeItemsFilter(ResourceId: request.ResourceId, IsDispanserization: false),
            cancellationToken: cancellationToken);

        return timeItems
            .Where(ti => ti.ResourceId == request.ResourceId)
            .GroupBy(ti => ti.From.Date)
            .Where(g => g.Any())
            .Select(g => new DateItem(
                g.Select(ti => ti.From).Min(),
                g.Select(ti => ti.To).Max(),
                g.Count(ti => !timeItemIds.Contains(ti.Id)),
                request.ResourceId
            ))
            .OrderBy(di => di.ResourceId)
            .ThenBy(di => di.From)
            .ToArray();
    }
}
