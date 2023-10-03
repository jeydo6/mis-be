using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetDispanserizationDateItemsHandler : IRequestHandler<GetDispanserizationDateItemsQuery, DateItem[]>
{
    private readonly IResearchesRepository _researchesRepository;
    private readonly ITimeItemsRepository _timeItemsRepository;
    private readonly IVisitItemsRepository _visitItemsRepository;

    public GetDispanserizationDateItemsHandler(
        IResearchesRepository researchesRepository,
        ITimeItemsRepository timeItemsRepository,
        IVisitItemsRepository visitItemsRepository)
    {
        _researchesRepository = researchesRepository;
        _timeItemsRepository = timeItemsRepository;
        _visitItemsRepository = visitItemsRepository;
    }

    public async Task<DateItem[]> Handle(GetDispanserizationDateItemsQuery request, CancellationToken cancellationToken)
    {
        var visitItems = await _visitItemsRepository.GetAll(request.From, request.To, cancellationToken: cancellationToken);
        var timeItemIds = visitItems.Select(vi => vi.TimeItemId).ToHashSet();

        var researches = await _researchesRepository.GetAll(cancellationToken);
        var resourceIds = researches
            .Where(r => r.IsDispanserization)
            .Select(r => r.ResourceId)
            .ToHashSet();

        var timeItems = await _timeItemsRepository.GetAll(request.From, request.To, cancellationToken: cancellationToken);
        return timeItems
            .Where(ti => resourceIds.Contains(ti.ResourceId))
            .GroupBy(ti => (ti.ResourceId, ti.From.Date))
            .Where(g => g.Any())
            .Select(g => new DateItem(
                g.Select(ti => ti.From).Min(),
                g.Select(ti => ti.To).Max(),
                g.Key.ResourceId,
                g.Count(ti => !timeItemIds.Contains(ti.Id))
            ))
            .GroupBy(di => di.From.Date)
            .Where(g =>
            {
                var groupResourceIds = g.Select(di => di.ResourceId).ToHashSet();
                return resourceIds.All(resourceId => groupResourceIds.Contains(resourceId));
            })
            .SelectMany(g => g)
            .OrderBy(di => di.From)
            .ToArray();
    }
}
