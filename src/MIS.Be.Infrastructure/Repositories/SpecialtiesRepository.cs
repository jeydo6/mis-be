using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;
using MIS.Be.Infrastructure.DataContexts;

namespace MIS.Be.Infrastructure.Repositories;

internal sealed class SpecialtiesRepository : ISpecialtiesRepository
{
    private readonly DbContext _db;

    public SpecialtiesRepository(DbContext db) => _db = db;

    public Task<int> Create(Specialty item, CancellationToken cancellationToken = default)
        => _db.InsertWithInt32IdentityAsync(item, token: cancellationToken);

    public async Task<Specialty> Get(int id, CancellationToken cancellationToken = default)
    {
        var query =
            from s in _db.Specialties
            where
                s.IsActive &&
                s.Id == id
            select s;

        var result = await query.FirstOrDefaultAsync(token: cancellationToken);
        ArgumentNullException.ThrowIfNull(result);

        return result;
    }

    public async Task<Specialty[]> Get(int[] ids, CancellationToken cancellationToken = default)
    {
        var query =
            from s in _db.Specialties
            where
                s.IsActive &&
                ids.Contains(s.Id)
            select s;

        var result = await query.ToArrayAsync(token: cancellationToken);
        ArgumentOutOfRangeException.ThrowIfNotEqual(result.Length, ids.Length);

        return result;
    }

    public Task<Specialty[]> GetAll(CancellationToken cancellationToken = default)
    {
        var query =
            from s in _db.Specialties
            where s.IsActive
            select s;

        return query.ToArrayAsync(token: cancellationToken);
    }
}
