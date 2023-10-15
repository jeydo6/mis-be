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
            where vi.Id == id &&
                  vi.IsActive
            select vi;

        var result = await query.FirstOrDefaultAsync(token: cancellationToken);
        ArgumentNullException.ThrowIfNull(result);

        return result;
    }

    public Task<VisitItem[]> GetAll(DateTimeOffset from, DateTimeOffset to, GetAllVisitItemsFilter? filter = default, CancellationToken cancellationToken = default)
    {
        var specialtyId = filter?.SpecialtyId;
        var resourceId = filter?.ResourceId;
        var patientId = filter?.PatientId;

        var query =
            from vi in _db.VisitItems
            join ti in _db.TimeItems on vi.TimeItemId equals ti.Id
            join r in _db.Resources on ti.ResourceId equals r.Id
            where ti.From >= @from && ti.From <= to &&
                  r.IsActive &&
                  ti.IsActive &&
                  vi.IsActive &&
                  (!patientId.HasValue || vi.PatientId == patientId.Value) &&
                  (!resourceId.HasValue || ti.ResourceId == resourceId.Value) &&
                  (!specialtyId.HasValue || r.SpecialtyId == specialtyId.Value)
            select vi;

        return query.ToArrayAsync(token: cancellationToken);
    }
}
