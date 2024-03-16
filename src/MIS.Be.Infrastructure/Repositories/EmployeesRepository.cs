using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;
using MIS.Be.Infrastructure.DataContexts;

namespace MIS.Be.Infrastructure.Repositories;

internal sealed class EmployeesRepository : IEmployeesRepository
{
    private readonly DbContext _db;

    public EmployeesRepository(DbContext db) => _db = db;

    public Task<int> Create(Employee item, CancellationToken cancellationToken = default)
        => _db.InsertWithInt32IdentityAsync(item, token: cancellationToken);

    public async Task<Employee> Get(int id, CancellationToken cancellationToken = default)
    {
        var query =
            from e in _db.Employees
            where e.Id == id &&
                  e.IsActive
            select e;

        var result = await query.FirstOrDefaultAsync(token: cancellationToken);
        ArgumentNullException.ThrowIfNull(result);

        return result;
    }

    public async Task<Employee[]> Get(int[] ids, CancellationToken cancellationToken = default)
    {
        var query =
            from e in _db.Employees
            where ids.Contains(e.Id) &&
                  e.IsActive
            select e;

        var result = await query.ToArrayAsync(token: cancellationToken);
        ArgumentOutOfRangeException.ThrowIfNotEqual(result.Length, ids.Length);

        return result;
    }

    public Task<Employee[]> GetAll(CancellationToken cancellationToken = default)
    {
        var query =
            from e in _db.Employees
            where e.IsActive
            select e;

        return query.ToArrayAsync(token: cancellationToken);
    }
}
