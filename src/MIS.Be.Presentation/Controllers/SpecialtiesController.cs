using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MIS.Be.Application.Models;
using MIS.Be.Application.Queries;

namespace MIS.Be.Presentation.Controllers;

[ApiController]
[Route("specialties")]
public sealed class SpecialtiesController
{
    private readonly IMediator _mediator;

    public SpecialtiesController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public Task<Specialty[]> GetSpecialties([FromQuery] GetSpecialtiesQuery request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);

    [HttpGet("all")]
    public Task<Specialty[]> GetAllSpecialties([FromQuery] GetAllSpecialtiesQuery request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);

    [HttpGet("resources")]
    public Task<Resource[]> GetSpecialtyResources([FromQuery] GetSpecialtyResourcesQuery request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);
}
