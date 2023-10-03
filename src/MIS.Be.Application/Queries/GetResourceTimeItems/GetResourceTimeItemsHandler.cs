using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetResourceTimeItemsHandler : IRequestHandler<GetResourceTimeItemsQuery, TimeItem[]>
{
    private readonly ITimeItemsRepository _timeItemsRepository;
    private readonly IVisitItemsRepository _visitItemsRepository;

    public GetResourceTimeItemsHandler(
        ITimeItemsRepository timeItemsRepository,
        IVisitItemsRepository visitItemsRepository)
    {
        _timeItemsRepository = timeItemsRepository;
        _visitItemsRepository = visitItemsRepository;
    }

    public async Task<TimeItem[]> Handle(GetResourceTimeItemsQuery request, CancellationToken cancellationToken)
    {
        var visitItems = await _visitItemsRepository.GetAll(request.From, request.To, resourceId: request.ResourceId, cancellationToken: cancellationToken);
        var timeItemIds = visitItems.Select(vi => vi.TimeItemId).ToHashSet();

        var timeItems = await _timeItemsRepository.GetAll(request.From, request.To, request.ResourceId, cancellationToken: cancellationToken);
        return timeItems
            .Where(ti => !timeItemIds.Contains(ti.Id) && ti.ResourceId == request.ResourceId)
            .Select(MappingExtension.Map)
            .OrderBy(di => di.From)
            .ToArray();
    }
}
