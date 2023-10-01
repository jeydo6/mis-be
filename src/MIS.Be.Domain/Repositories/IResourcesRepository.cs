using System.Threading;
using System.Threading.Tasks;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories;

public interface IResourcesRepository
{
	Task<int> Create(Resource item, CancellationToken cancellationToken = default);
	Task<Resource> Get(int id, CancellationToken cancellationToken = default);
	Task<Resource[]> GetAll(CancellationToken cancellationToken = default);
}
