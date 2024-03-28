using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MIS.Be.Application.Models;
using MIS.Be.Application.Queries;

namespace MIS.Be.Presentation.Controllers;

[ApiController]
[Route("dateItems")]
public class DateItemsController
{
    private readonly IMediator _mediator;

    public DateItemsController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("dispanserization")]
    public Task<DateItem[]> GetDispanserizationDateItems([FromQuery] GetDispanserizationDateItemsQuery request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);
}
