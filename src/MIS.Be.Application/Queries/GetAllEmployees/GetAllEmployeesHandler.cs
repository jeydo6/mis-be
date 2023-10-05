using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetAllEmployeesHandler : IRequestHandler<GetAllEmployeesQuery, Employee[]>
{
    private readonly IEmployeesRepository _repository;

    public GetAllEmployeesHandler(IEmployeesRepository repository)
        => _repository = repository;

    public async Task<Employee[]> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _repository.GetAll(cancellationToken);
        return employees
            .Select(MappingExtension.Map)
            .OrderBy(s => s.Id)
            .ToArray();
    }
}
