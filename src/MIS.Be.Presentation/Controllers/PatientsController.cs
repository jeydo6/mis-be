using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MIS.Be.Application.Models;
using MIS.Be.Application.Queries;

namespace MIS.Be.Presentation.Controllers;

[ApiController]
[Route("patients")]
public class PatientsController
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("find")]
    public Task<Patient?> FindPatient([FromQuery] FindPatientQuery request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);
}
