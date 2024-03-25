using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MIS.Be.Application.Models;
using MIS.Be.Application.Queries;

namespace MIS.Be.Presentation.Controllers;

[ApiController]
[Route("timeItems")]
public class TimeItemsController
{
    private readonly IMediator _mediator;

    public TimeItemsController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public Task<TimeItem[]> GetTimeItems([FromQuery] GetTimeItemsQuery request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);

    [HttpGet("resource")]
    public Task<TimeItem[]> GetResourceTimeItems([FromQuery] GetResourceTimeItemsQuery request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);
}
