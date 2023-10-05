using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MIS.Be.Application.Models;
using MIS.Be.Application.Queries;

namespace MIS.Be.Presentation.Controllers;

[ApiController]
[Route("employees")]
public class EmployeesController
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public Task<Employee[]> GetAllEmployees([FromQuery] GetAllEmployeesQuery request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);
}
