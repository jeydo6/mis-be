using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;
using MIS.Be.Infrastructure.DataContexts;

namespace MIS.Be.Infrastructure.Repositories;

internal sealed class VisitItemsRepository : IVisitItemsRepository
{
    private readonly DbContext _db;

    public VisitItemsRepository(DbContext db) => _db = db;

    public Task<int> Create(VisitItem item, CancellationToken cancellationToken = default)
        => _db.InsertWithInt32IdentityAsync(item, token: cancellationToken);

    public async Task<VisitItem> Get(int id, CancellationToken cancellationToken = default)
    {
        var query =
            from vi in _db.VisitItems
            where
                vi.IsActive &&
                vi.Id == id
            select vi;

        var result = await query.FirstOrDefaultAsync(token: cancellationToken);
        ArgumentNullException.ThrowIfNull(result);

        return result;
    }

    public Task<VisitItem[]> GetAll(int patientId, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default)
    {
        var query =
            from vi in _db.VisitItems
            join ti in _db.TimeItems on vi.TimeItemId equals ti.Id
            join r in _db.Resources on ti.ResourceId equals r.Id
            where
                r.IsActive &&
                ti.IsActive &&
                ti.From >= @from && ti.From <= to &&
                vi.IsActive &&
                vi.PatientId == patientId
            select vi;

        return query.ToArrayAsync(token: cancellationToken);
    }
}
