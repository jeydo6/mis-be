using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MIS.Be.Application.Commands;
using MIS.Be.Application.Models;
using MIS.Be.Application.Queries;

namespace MIS.Be.Presentation.Controllers;

[ApiController]
[Route("visitItems")]
public class VisitItemsController
{
    private readonly IMediator _mediator;

    public VisitItemsController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost]
    public Task CreateVisitItem([FromBody] CreateVisitItemCommand request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);

    [HttpGet("patient")]
    public Task<VisitItem[]> GetPatientVisitItems([FromQuery] GetPatientVisitItemsQuery request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);
}
