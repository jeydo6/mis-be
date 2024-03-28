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

    public GetResourceTimeItemsHandler(ITimeItemsRepository timeItemsRepository)
        => _timeItemsRepository = timeItemsRepository;

    public async Task<TimeItem[]> Handle(GetResourceTimeItemsQuery request, CancellationToken cancellationToken)
    {
        var timeItems = await _timeItemsRepository.GetAll(
            new int[] { request.ResourceId },
            request.From, request.To,
            cancellationToken: cancellationToken);

        return timeItems
            .Select(MappingExtension.Map)
            .OrderBy(di => di.From)
            .ToArray();
    }
}
