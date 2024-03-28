using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetPatientDispanserizationsHandler : IRequestHandler<GetPatientDispanserizationsQuery, Dispanserization[]>
{
    private readonly IDispanserizationsRepository _repository;

    public GetPatientDispanserizationsHandler(IDispanserizationsRepository repository)
        => _repository = repository;

    public async Task<Dispanserization[]> Handle(GetPatientDispanserizationsQuery request, CancellationToken cancellationToken)
    {
        var dispanserizations = await _repository.GetAll(request.PatientId, cancellationToken);

        return dispanserizations
            .Select(MappingExtension.Map)
            .OrderBy(d => d.Id)
            .ToArray();
    }
}
