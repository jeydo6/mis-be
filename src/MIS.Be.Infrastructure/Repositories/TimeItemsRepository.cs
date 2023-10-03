using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;
using MIS.Be.Infrastructure.DataContexts;

namespace MIS.Be.Infrastructure.Repositories;

internal sealed class TimeItemsRepository : ITimeItemsRepository
{
    private readonly DbContext _db;

    public TimeItemsRepository(DbContext db) => _db = db;

    public Task<int> Create(TimeItem item, CancellationToken cancellationToken = default)
        => _db.InsertWithInt32IdentityAsync(item, token: cancellationToken);

    public async Task<TimeItem> Get(int id, CancellationToken cancellationToken = default)
    {
        var query =
            from ti in _db.TimeItems
            where ti.Id == id &&
                  ti.IsActive
            select ti;

        var result = await query.FirstOrDefaultAsync(token: cancellationToken);
        ArgumentNullException.ThrowIfNull(result);

        return result;
    }

    public Task<TimeItem[]> Get(int[] ids, CancellationToken cancellationToken = default)
    {
        var query =
            from ti in _db.TimeItems
            where ids.Contains(ti.Id) &&
                  ti.IsActive
            select ti;

        return query.ToArrayAsync(token: cancellationToken);
    }

    public Task<TimeItem[]> GetAll(DateTimeOffset from, DateTimeOffset to, int? resourceId = default, CancellationToken cancellationToken = default)
    {
        var query =
            from ti in _db.TimeItems
            where ti.From >= @from && ti.From <= to &&
                  ti.IsActive &&
                  (!resourceId.HasValue || ti.ResourceId == resourceId.Value)
            select ti;

        return query.ToArrayAsync(token: cancellationToken);
    }
}
