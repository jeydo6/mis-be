using System.Threading;
using System.Threading.Tasks;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories;

public interface IResearchesRepository
{
    Task<Research[]> GetAll(CancellationToken cancellationToken = default);
}
