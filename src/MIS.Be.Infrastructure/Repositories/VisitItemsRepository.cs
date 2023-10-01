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
			where vi.Id == id &&
                  vi.IsActive
			select vi;

		var result = await query.FirstOrDefaultAsync(token: cancellationToken);
		ArgumentNullException.ThrowIfNull(result);

		return result;
	}

    public Task<VisitItem[]> GetAll(DateTimeOffset from, DateTimeOffset to, int? resourceId = default, int? patientId = default, CancellationToken cancellationToken = default)
    {
        var query =
            from vi in _db.VisitItems
            join ti in _db.TimeItems on vi.TimeItemId equals ti.Id
            where ti.From >= @from && ti.From <= to &&
                  ti.IsActive &&
                  vi.IsActive &&
                  (!resourceId.HasValue || ti.ResourceId == resourceId.Value) &&
                  (!patientId.HasValue || vi.PatientId == patientId.Value)
            select vi;

        return query.ToArrayAsync(token: cancellationToken);
    }
}
