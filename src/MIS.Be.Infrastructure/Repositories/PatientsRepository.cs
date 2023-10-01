using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;
using MIS.Be.Infrastructure.DataContexts;

namespace MIS.Be.Infrastructure.Repositories;

internal sealed class PatientsRepository : IPatientsRepository
{
	private readonly DbContext _db;

	public PatientsRepository(DbContext db) => _db = db;

	public Task<int> Create(Patient item, CancellationToken cancellationToken = default)
		=> _db.InsertWithInt32IdentityAsync(item, token: cancellationToken);

	public async Task<Patient> Get(int id, CancellationToken cancellationToken = default)
	{
		var query =
			from p in _db.Patients
			where p.Id == id &&
                  p.IsActive
			select p;

		var result = await query.FirstOrDefaultAsync(token: cancellationToken);
		ArgumentNullException.ThrowIfNull(result);

		return result;
	}

	public Task<Patient?> Find(string code, int birthYear, CancellationToken cancellationToken = default)
	{
		var query =
			from p in _db.Patients
			where p.Code == code &&
			      p.BirthDate.Year == birthYear &&
			      p.IsActive
			select p;

		return query.FirstOrDefaultAsync(token: cancellationToken);
	}
}
