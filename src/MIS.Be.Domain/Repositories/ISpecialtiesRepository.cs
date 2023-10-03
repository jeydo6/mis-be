using System.Threading;
using System.Threading.Tasks;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories;

public interface ISpecialtiesRepository
{
    Task<int> Create(Specialty item, CancellationToken cancellationToken = default);
    Task<Specialty> Get(int id, CancellationToken cancellationToken = default);
    Task<Specialty[]> GetAll(CancellationToken cancellationToken = default);
}
