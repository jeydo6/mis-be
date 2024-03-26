using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class FindPatientHandler : IRequestHandler<FindPatientQuery, Patient?>
{
    private readonly IPatientsRepository _repository;

    public FindPatientHandler(IPatientsRepository repository)
        => _repository = repository;

    public async Task<Patient?> Handle(FindPatientQuery request, CancellationToken cancellationToken)
    {
        var patient = await _repository.Find(request.Code, request.BirthYear, cancellationToken);

        return patient?.Map();
    }
}
