using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetEmployeesHandler : IRequestHandler<GetEmployeesQuery, Employee[]>
{
    private readonly IEmployeesRepository _repository;

    public GetEmployeesHandler(IEmployeesRepository repository)
        => _repository = repository;

    public async Task<Employee[]> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _repository.Get(request.Ids, cancellationToken);

        return employees
            .Select(MappingExtension.Map)
            .OrderBy(s => s.Id)
            .ToArray();
    }
}
