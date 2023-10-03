using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;
using MIS.Be.Infrastructure.DataContexts;

namespace MIS.Be.Infrastructure.Repositories;

internal sealed class DispanserizationsRepository : IDispanserizationsRepository
{
    private readonly DbContext _db;

    public DispanserizationsRepository(DbContext db) => _db = db;

    public Task<int> Create(Dispanserization item, CancellationToken cancellationToken = default)
        => _db.InsertWithInt32IdentityAsync(item, token: cancellationToken);

    public async Task<Dispanserization> Get(int id, CancellationToken cancellationToken = default)
    {
        var query =
            from d in _db.Dispanserizations
            where d.Id == id &&
                  d.IsActive
            select d;

        var result = await query.FirstOrDefaultAsync(token: cancellationToken);
        ArgumentNullException.ThrowIfNull(result);

        return result;
    }

    public Task<Dispanserization[]> GetAll(int patientId, CancellationToken cancellationToken = default)
    {
        var query =
            from d in _db.Dispanserizations
            where d.PatientId == patientId &&
                  d.IsActive
            select d;

        return query.ToArrayAsync(token: cancellationToken);
    }
}
