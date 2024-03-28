using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetDispanserizationDateItemsHandler : IRequestHandler<GetDispanserizationDateItemsQuery, DateItem[]>
{
    private readonly IResearchesRepository _researchesRepository;
    private readonly IDateItemsRepository _dateItemsRepository;

    public GetDispanserizationDateItemsHandler(
        IResearchesRepository researchesRepository,
        IDateItemsRepository dateItemsRepository
    )
    {
        _dateItemsRepository = dateItemsRepository;
        _researchesRepository = researchesRepository;
    }

    public async Task<DateItem[]> Handle(GetDispanserizationDateItemsQuery request, CancellationToken cancellationToken)
    {
        var researches = await _researchesRepository.GetAll(cancellationToken);
        var resourceIds = researches
            .Where(r => r.IsDispanserization)
            .Select(r => r.ResourceId)
            .ToHashSet();

        var dateItems = await _dateItemsRepository.GetAll(resourceIds.ToArray(), request.From, request.To, cancellationToken: cancellationToken);

        return dateItems
            .Select(MappingExtension.Map)
            .GroupBy(di => di.From.Date)
            .Where(g =>
            {
                var groupResourceIds = g.Select(di => di.ResourceId).ToHashSet();
                return resourceIds.All(resourceId => groupResourceIds.Contains(resourceId));
            })
            .SelectMany(g => g)
            .OrderBy(di => di.ResourceId)
            .ThenBy(di => di.From)
            .ToArray();
    }
}
