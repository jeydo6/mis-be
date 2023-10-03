using System.Threading;
using System.Threading.Tasks;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories;

public interface IPatientsRepository
{
    Task<int> Create(Patient item, CancellationToken cancellationToken = default);
    Task<Patient> Get(int id, CancellationToken cancellationToken = default);
    Task<Patient?> Find(string code, int birthYear, CancellationToken cancellationToken = default);
}
