using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetSpecialtiesHandler : IRequestHandler<GetSpecialtiesQuery, Specialty[]>
{
    private readonly ISpecialtiesRepository _repository;

    public GetSpecialtiesHandler(ISpecialtiesRepository repository)
        => _repository = repository;

    public async Task<Specialty[]> Handle(GetSpecialtiesQuery request, CancellationToken cancellationToken)
    {
        var specialties = await _repository.Get(request.Ids, cancellationToken);

        return specialties
            .Select(MappingExtension.Map)
            .OrderBy(s => s.Id)
            .ToArray();
    }
}
