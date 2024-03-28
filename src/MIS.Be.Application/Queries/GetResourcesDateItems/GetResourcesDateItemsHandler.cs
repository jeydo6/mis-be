using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetResourcesDateItemsHandler : IRequestHandler<GetResourcesDateItemsQuery, DateItem[]>
{
    private readonly IDateItemsRepository _dateItemsRepository;

    public GetResourcesDateItemsHandler(IDateItemsRepository dateItemsRepository)
        => _dateItemsRepository = dateItemsRepository;

    public async Task<DateItem[]> Handle(GetResourcesDateItemsQuery request, CancellationToken cancellationToken)
    {
        var dateItems = await _dateItemsRepository.GetAll(request.ResourceIds, request.From, request.To, cancellationToken);

        return dateItems
            .Where(di => di.TimeItemsCount > 0)
            .Select(MappingExtension.Map)
            .OrderBy(di => di.ResourceId)
            .ThenBy(di => di.From)
            .ToArray();
    }
}
