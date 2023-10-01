using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries.GetAllResources;

internal sealed class GetAllResourcesHandler : IRequestHandler<GetAllResourcesQuery, Resource[]>
{
    private readonly IResourcesRepository _repository;

    public GetAllResourcesHandler(IResourcesRepository repository)
        => _repository = repository;

    public async Task<Resource[]> Handle(GetAllResourcesQuery request, CancellationToken cancellationToken)
    {
        var resources = await _repository.GetAll(cancellationToken);
        return resources
            .Select(MappingExtension.Map)
            .ToArray();
    }
}
