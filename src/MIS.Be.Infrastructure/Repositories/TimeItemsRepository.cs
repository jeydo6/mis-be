using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Filters;
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

    public async Task<TimeItem[]> Get(int[] ids, CancellationToken cancellationToken = default)
    {
        var query =
            from ti in _db.TimeItems
            where ids.Contains(ti.Id) &&
                  ti.IsActive
            select ti;

        var result = await query.ToArrayAsync(token: cancellationToken);
        ArgumentOutOfRangeException.ThrowIfNotEqual(result.Length, ids.Length);

        return result;
    }

    public Task<TimeItem[]> GetAll(DateTimeOffset from, DateTimeOffset to, GetAllTimeItemsFilter? filter = default, CancellationToken cancellationToken = default)
    {
        var specialtyId = filter?.SpecialtyId;
        var resourceId = filter?.ResourceId;
        var isDispanserization = filter?.IsDispanserization;

        var query =
            from ti in _db.TimeItems
            join r in _db.Resources on ti.ResourceId equals r.Id
            where ti.From >= @from && ti.From <= to &&
                  r.IsActive &&
                  ti.IsActive &&
                  (!resourceId.HasValue || ti.ResourceId == resourceId.Value) &&
                  (!specialtyId.HasValue || r.SpecialtyId == specialtyId.Value) &&
                  (!isDispanserization.HasValue || r.IsDispanserization == isDispanserization.Value)
            select ti;

        return query.ToArrayAsync(token: cancellationToken);
    }
}
