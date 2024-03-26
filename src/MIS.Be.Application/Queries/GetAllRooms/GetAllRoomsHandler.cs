using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetAllRoomsHandler : IRequestHandler<GetAllRoomsQuery, Room[]>
{
    private readonly IRoomsRepository _repository;

    public GetAllRoomsHandler(IRoomsRepository repository)
        => _repository = repository;

    public async Task<Room[]> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await _repository.GetAll(cancellationToken);

        return rooms
            .Select(MappingExtension.Map)
            .OrderBy(s => s.Id)
            .ToArray();
    }
}
