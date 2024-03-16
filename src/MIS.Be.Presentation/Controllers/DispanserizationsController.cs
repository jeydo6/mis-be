using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MIS.Be.Application.Commands;
using MIS.Be.Application.Models;
using MIS.Be.Application.Queries;

namespace MIS.Be.Presentation.Controllers;

[ApiController]
[Route("dispanserizations")]
public class DispanserizationsController
{
    private readonly IMediator _mediator;

    public DispanserizationsController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public Task CreateDispanserization([FromBody] CreateDispanserizationCommand request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);

    [HttpGet("all")]
    public Task<Dispanserization[]> GetAllDispanserizations([FromQuery] GetAllDispanserizationsQuery request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);
}
