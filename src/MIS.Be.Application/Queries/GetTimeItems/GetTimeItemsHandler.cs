using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetTimeItemsHandler : IRequestHandler<GetTimeItemsQuery, TimeItem[]>
{
    private readonly ITimeItemsRepository _timeItemsRepository;

    public GetTimeItemsHandler(ITimeItemsRepository timeItemsRepository)
        => _timeItemsRepository = timeItemsRepository;

    public async Task<TimeItem[]> Handle(GetTimeItemsQuery request, CancellationToken cancellationToken)
    {
        var timeItems = await _timeItemsRepository.Get(request.Ids, cancellationToken);

        return timeItems
            .Select(MappingExtension.Map)
            .OrderBy(s => s.Id)
            .ToArray();
    }
}
