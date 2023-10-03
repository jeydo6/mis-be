using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Repositories;
using MIS.Be.Infrastructure.DataContexts;

namespace MIS.Be.Infrastructure.Repositories;

internal sealed class ResearchesRepository : IResearchesRepository
{
    private readonly DbContext _db;

    public ResearchesRepository(DbContext db) => _db = db;

    public Task<Research[]> GetAll(CancellationToken cancellationToken = default)
    {
        var query =
            from r in _db.Researches
            where r.IsActive
            select r;

        return query.ToArrayAsync(token: cancellationToken);
    }
}
