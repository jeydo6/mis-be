using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MIS.Be.Application.Models;
using MIS.Be.Application.Queries;

namespace MIS.Be.Presentation.Controllers;

[ApiController]
[Route("rooms")]
public sealed class RoomsController
{
    private readonly IMediator _mediator;

    public RoomsController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public Task<Room[]> GetAllRooms([FromQuery] GetAllRoomsQuery request, CancellationToken cancellationToken)
        => _mediator.Send(request, cancellationToken);
}
