using System.Threading;
using System.Threading.Tasks;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories;

public interface IRoomsRepository
{
    Task<int> Create(Room item, CancellationToken cancellationToken = default);
    Task<Room> Get(int id, CancellationToken cancellationToken = default);
    Task<Room[]> GetAll(CancellationToken cancellationToken = default);
}
