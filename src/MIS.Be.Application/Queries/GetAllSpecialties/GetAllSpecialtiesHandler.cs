using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetAllSpecialtiesHandler : IRequestHandler<GetAllSpecialtiesQuery, Specialty[]>
{
    private readonly ISpecialtiesRepository _repository;

    public GetAllSpecialtiesHandler(ISpecialtiesRepository repository)
        => _repository = repository;

    public async Task<Specialty[]> Handle(GetAllSpecialtiesQuery request, CancellationToken cancellationToken)
    {
        var specialties = await _repository.GetAll(cancellationToken);
        return specialties
            .Select(MappingExtension.Map)
            .OrderBy(s => s.Id)
            .ToArray();
    }
}
