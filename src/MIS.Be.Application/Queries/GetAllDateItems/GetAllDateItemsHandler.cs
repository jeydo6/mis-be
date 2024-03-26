using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetAllDateItemsHandler : IRequestHandler<GetAllDateItemsQuery, DateItem[]>
{
    private readonly ITimeItemsRepository _timeItemsRepository;
    private readonly IVisitItemsRepository _visitItemsRepository;

    public GetAllDateItemsHandler(
        ITimeItemsRepository timeItemsRepository,
        IVisitItemsRepository visitItemsRepository)
    {
        _timeItemsRepository = timeItemsRepository;
        _visitItemsRepository = visitItemsRepository;
    }

    public async Task<DateItem[]> Handle(GetAllDateItemsQuery request, CancellationToken cancellationToken)
    {
        var visitItems = await _visitItemsRepository.GetAll(request.From, request.To, cancellationToken: cancellationToken);
        var timeItemIds = visitItems.Select(vi => vi.TimeItemId).ToHashSet();

        var timeItems = await _timeItemsRepository.GetAll(request.From, request.To, cancellationToken: cancellationToken);

        return timeItems
            .GroupBy(ti => (ti.ResourceId, ti.From.Date))
            .Where(g => g.Any())
            .Select(g => new DateItem(
                g.Select(ti => ti.From).Min(),
                g.Select(ti => ti.To).Max(),
                g.Count(ti => !timeItemIds.Contains(ti.Id)),
                g.Key.ResourceId
            ))
            .OrderBy(di => di.ResourceId)
            .ThenBy(di => di.From)
            .ToArray();
    }
}
