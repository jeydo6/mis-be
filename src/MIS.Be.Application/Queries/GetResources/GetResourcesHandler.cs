using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetResourcesHandler : IRequestHandler<GetResourcesQuery, Resource[]>
{
    private readonly IResourcesRepository _repository;

    public GetResourcesHandler(IResourcesRepository repository)
        => _repository = repository;

    public async Task<Resource[]> Handle(GetResourcesQuery request, CancellationToken cancellationToken)
    {
        var resources = await _repository.Get(request.Ids, cancellationToken);

        return resources
            .Select(MappingExtension.Map)
            .OrderBy(s => s.Id)
            .ToArray();
    }
}
