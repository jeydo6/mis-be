using System.Threading;
using System.Threading.Tasks;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories;

public interface IEmployeesRepository
{
    Task<int> Create(Employee item, CancellationToken cancellationToken = default);
    Task<Employee> Get(int id, CancellationToken cancellationToken = default);
    Task<Employee[]> GetAll(CancellationToken cancellationToken = default);
}
