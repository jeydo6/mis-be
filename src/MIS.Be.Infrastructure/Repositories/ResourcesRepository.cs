using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;
using MIS.Be.Infrastructure.DataContexts;

namespace MIS.Be.Infrastructure.Repositories;

internal sealed class ResourcesRepository : IResourcesRepository
{
    private readonly DbContext _db;

    public ResourcesRepository(DbContext db) => _db = db;

    public Task<int> Create(Resource item, CancellationToken cancellationToken = default)
        => _db.InsertWithInt32IdentityAsync(item, token: cancellationToken);

    public async Task<Resource[]> Get(int[] ids, CancellationToken cancellationToken = default)
    {
        var query =
            from r in _db.Resources
            where
                r.IsActive &&
                ids.Contains(r.Id)
            select r;

        var result = await query.ToArrayAsync(token: cancellationToken);
        ArgumentOutOfRangeException.ThrowIfNotEqual(result.Length, ids.Length);

        return result;
    }

    public Task<Resource[]> GetAll(int? specialtyId = default, CancellationToken cancellationToken = default)
    {
        var query =
            from r in _db.Resources
            where
                r.IsActive &&
                (!specialtyId.HasValue || r.SpecialtyId == specialtyId.Value)
            select r;

        return query.ToArrayAsync(token: cancellationToken);
    }
}
