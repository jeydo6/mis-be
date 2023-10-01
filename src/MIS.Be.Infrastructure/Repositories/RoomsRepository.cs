using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;
using MIS.Be.Infrastructure.DataContexts;

namespace MIS.Be.Infrastructure.Repositories;

internal sealed class RoomsRepository : IRoomsRepository
{
	private readonly DbContext _db;

	public RoomsRepository(DbContext db) => _db = db;

	public Task<int> Create(Room item, CancellationToken cancellationToken = default)
		=> _db.InsertWithInt32IdentityAsync(item, token: cancellationToken);

	public async Task<Room> Get(int id, CancellationToken cancellationToken = default)
	{
		var query =
			from r in _db.Rooms
			where r.Id == id &&
                  r.IsActive
			select r;

		var result = await query.FirstOrDefaultAsync(token: cancellationToken);
		ArgumentNullException.ThrowIfNull(result);

		return result;
	}

	public Task<Room[]> GetAll(CancellationToken cancellationToken = default)
	{
		var query =
			from r in _db.Rooms
            where r.IsActive
			select r;

		return query.ToArrayAsync(token: cancellationToken);
	}
}
