using System;
using System.Threading;
using System.Threading.Tasks;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Filters;

namespace MIS.Be.Domain.Repositories;

public interface ITimeItemsRepository
{
    Task<int> Create(TimeItem item, CancellationToken cancellationToken = default);
    Task<TimeItem> Get(int id, CancellationToken cancellationToken = default);
    Task<TimeItem[]> Get(int[] ids, CancellationToken cancellationToken = default);
    Task<TimeItem[]> GetAll(DateTimeOffset from, DateTimeOffset to, GetAllTimeItemsFilter? filter = default, CancellationToken cancellationToken = default);
}
