using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Filters;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetSpecialtyResourcesHandler : IRequestHandler<GetSpecialtyResourcesQuery, Resource[]>
{
    private readonly IResourcesRepository _repository;

    public GetSpecialtyResourcesHandler(IResourcesRepository repository)
        => _repository = repository;

    public async Task<Resource[]> Handle(GetSpecialtyResourcesQuery request, CancellationToken cancellationToken)
    {
        var resources = await _repository.GetAll(
            request.SpecialtyId,
            cancellationToken: cancellationToken);

        return resources
            .Select(MappingExtension.Map)
            .OrderBy(r => r.Id)
            .ToArray();
    }
}
