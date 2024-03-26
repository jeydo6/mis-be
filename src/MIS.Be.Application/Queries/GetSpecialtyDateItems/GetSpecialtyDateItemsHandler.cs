using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Filters;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetSpecialtyDateItemsHandler : IRequestHandler<GetSpecialtyDateItemsQuery, DateItem[]>
{
    private readonly ITimeItemsRepository _timeItemsRepository;
    private readonly IVisitItemsRepository _visitItemsRepository;

    public GetSpecialtyDateItemsHandler(
        ITimeItemsRepository timeItemsRepository,
        IVisitItemsRepository visitItemsRepository)
    {
        _timeItemsRepository = timeItemsRepository;
        _visitItemsRepository = visitItemsRepository;
    }

    public async Task<DateItem[]> Handle(GetSpecialtyDateItemsQuery request, CancellationToken cancellationToken)
    {
        var visitItems = await _visitItemsRepository.GetAll(request.From, request.To,
            filter: new GetAllVisitItemsFilter(SpecialtyId: request.SpecialtyId),
            cancellationToken: cancellationToken);
        var timeItemIds = visitItems.Select(vi => vi.TimeItemId).ToHashSet();

        var timeItems = await _timeItemsRepository.GetAll(request.From, request.To,
            filter: new GetAllTimeItemsFilter(SpecialtyId: request.SpecialtyId, IsDispanserization: false),
            cancellationToken: cancellationToken);

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
