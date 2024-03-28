using System;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;
using MIS.Be.Infrastructure.DataContexts;

namespace MIS.Be.Infrastructure.Repositories;

internal sealed class DateItemsRepository : IDateItemsRepository
{
    private readonly DbContext _db;

    public DateItemsRepository(DbContext db) => _db = db;

    public Task<DateItem[]> GetAll(int[] resourceIds, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default)
    {
        const string sql =
"""
SELECT
    MIN(ti."From") AS "From",
    MAX(ti."From") AS "To",
    COUNT(ti) AS "TimeItemsCount",
    COUNT(vi) AS "VisitItemsCount",
    ti."ResourceId"
FROM
    "TimeItem" AS ti
    LEFT OUTER JOIN "VisitItem" AS vi ON ti."Id" = vi."TimeItemId" AND vi."IsActive"
WHERE
    ti."IsActive"
    AND ti."From" BETWEEN {0} AND {1}
    AND ti."ResourceId" = ANY({2})
GROUP BY
    ti."ResourceId",
    CAST(ti."From" AS Date)
""";

        var query = _db.FromSql<DateItem>(sql, from, to, resourceIds);

        return query.ToArrayAsync(cancellationToken);
    }
}
