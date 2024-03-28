using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MIS.Be.Application.Commands;

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
}
