using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetAllResearchesHandler : IRequestHandler<GetAllResearchesQuery, Research[]>
{
    private readonly IResearchesRepository _repository;

    public GetAllResearchesHandler(IResearchesRepository repository)
        => _repository = repository;

    public async Task<Research[]> Handle(GetAllResearchesQuery request, CancellationToken cancellationToken)
    {
        var researches = await _repository.GetAll(cancellationToken);

        return researches
            .Select(MappingExtension.Map)
            .OrderBy(s => s.Id)
            .ToArray();
    }
}
