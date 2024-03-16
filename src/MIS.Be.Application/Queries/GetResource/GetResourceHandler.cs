using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetResourceHandler : IRequestHandler<GetResourceQuery, Resource>
{
    private readonly IResourcesRepository _repository;

    public GetResourceHandler(IResourcesRepository repository)
        => _repository = repository;

    public async Task<Resource> Handle(GetResourceQuery request, CancellationToken cancellationToken)
    {
        var resource = await _repository.Get(request.Id, cancellationToken);
        return resource.Map();
    }
}
