using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetPatientHandler : IRequestHandler<GetPatientQuery, Patient>
{
    private readonly IPatientsRepository _repository;

    public GetPatientHandler(IPatientsRepository repository)
        => _repository = repository;

    public async Task<Patient> Handle(GetPatientQuery request, CancellationToken cancellationToken)
    {
        var patient = await _repository.Get(request.Id, cancellationToken);

        return patient.Map();
    }
}
