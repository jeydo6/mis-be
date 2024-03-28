using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetResourceInfosHandler : IRequestHandler<GetResourceInfosQuery, ResourceInfo[]>
{
    private readonly IDateItemsRepository _dateItemsRepository;

    public GetResourceInfosHandler(IDateItemsRepository dateItemsRepository)
        => _dateItemsRepository = dateItemsRepository;

    public async Task<ResourceInfo[]> Handle(GetResourceInfosQuery request, CancellationToken cancellationToken)
    {
        var timeItemsCounts = new Dictionary<int, int>();
        var visitItemsCounts = new Dictionary<int, int>();

        var dateItems = await _dateItemsRepository.GetAll(request.ResourceIds, request.From, request.To, cancellationToken);
        foreach (var dateItem in dateItems)
        {
            timeItemsCounts.TryAdd(dateItem.ResourceId, 0);
            timeItemsCounts[dateItem.ResourceId] += dateItem.TimeItemsCount;

            visitItemsCounts.TryAdd(dateItem.ResourceId, 0);
            visitItemsCounts[dateItem.ResourceId] += dateItem.VisitItemsCount;
        }

        var result = new List<ResourceInfo>();
        foreach (var resourceId in request.ResourceIds)
        {
            if (!timeItemsCounts.TryGetValue(resourceId, out var timeItemsCount))
                continue;

            if (!visitItemsCounts.TryGetValue(resourceId, out var visitItemsCount))
                continue;

            result.Add(new ResourceInfo(resourceId, timeItemsCount, visitItemsCount));
        }

        return result.ToArray();
    }
}
