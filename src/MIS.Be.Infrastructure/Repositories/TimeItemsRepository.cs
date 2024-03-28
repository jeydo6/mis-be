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
            where
                ti.IsActive &&
                ti.Id == id

            select ti;

        var result = await query.FirstOrDefaultAsync(token: cancellationToken);
        ArgumentNullException.ThrowIfNull(result);

        return result;
    }

    public async Task<TimeItem[]> Get(int[] ids, CancellationToken cancellationToken = default)
    {
        var query =
            from ti in _db.TimeItems
            where
                ti.IsActive &&
                ids.Contains(ti.Id)

            select ti;

        var result = await query.ToArrayAsync(token: cancellationToken);
        ArgumentOutOfRangeException.ThrowIfNotEqual(result.Length, ids.Length);

        return result;
    }

    public Task<TimeItem[]> GetAll(int[] resourceIds, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default)
    {
        const string sql =
"""
SELECT
    ti."Id",
    ti."IsActive",
    ti."From",
    ti."To",
    ti."ResourceId"
FROM
    "TimeItem" AS ti
    LEFT OUTER JOIN "VisitItem" AS vi ON ti."Id" = vi."TimeItemId" AND vi."IsActive"
WHERE
    ti."IsActive"
    AND ti."From" BETWEEN {0} AND {1}
    AND ti."ResourceId" = ANY({2})
""";

        var query = _db.FromSql<TimeItem>(sql, from, to, resourceIds);

        return query.ToArrayAsync(cancellationToken);
    }
}
