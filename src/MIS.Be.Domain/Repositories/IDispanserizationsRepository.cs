using System.Threading;
using System.Threading.Tasks;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories;

public interface IDispanserizationsRepository
{
    Task<int> Create(Dispanserization item, CancellationToken cancellationToken = default);
    Task<Dispanserization> Get(int id, CancellationToken cancellationToken = default);
    Task<Dispanserization[]> GetAll(int patientId, CancellationToken cancellationToken = default);
}
