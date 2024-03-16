using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MIS.Be.Application.Models;
using MIS.Be.Application.Queries;

namespace MIS.Be.Presentation.Controllers;

[ApiController]
[Route("resources")]
public sealed class ResourcesController
{
    private readonly IMediator _mediator;

    public ResourcesController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public Task<Resource[]> GetResources([FromQuery] GetResourcesQuery request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);

    [HttpGet("all")]
    public Task<Resource[]> GetAllResources([FromQuery] GetAllResourcesQuery request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);

    [HttpGet("specialty")]
    public Task<Resource[]> GetSpecialtyResources([FromQuery] GetSpecialtyResourcesQuery request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);
}
