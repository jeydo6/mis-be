using System.Threading;
using System.Threading.Tasks;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Filters;

namespace MIS.Be.Domain.Repositories;

public interface IResourcesRepository
{
    Task<int> Create(Resource item, CancellationToken cancellationToken = default);
    Task<Resource[]> Get(int[] ids, CancellationToken cancellationToken = default);
    Task<Resource[]> GetAll(GetAllResourcesFilter? filter = default, CancellationToken cancellationToken = default);
}
