using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MIS.Be.Application.Models;
using MIS.Be.Application.Queries;

namespace MIS.Be.Presentation.Controllers;

[ApiController]
[Route("researches")]
public sealed class ResearchesController
{
    private readonly IMediator _mediator;

    public ResearchesController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("all")]
    public Task<Research[]> GetAllResearches([FromQuery] GetAllResearchesQuery request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);
}
