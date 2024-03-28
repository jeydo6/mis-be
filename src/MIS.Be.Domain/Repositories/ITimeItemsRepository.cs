using System;
using System.Threading;
using System.Threading.Tasks;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories;

public interface ITimeItemsRepository
{
    Task<int> Create(TimeItem item, CancellationToken cancellationToken = default);
    Task<TimeItem> Get(int id, CancellationToken cancellationToken = default);
    Task<TimeItem[]> Get(int[] ids, CancellationToken cancellationToken = default);
    Task<TimeItem[]> GetAll(int[] resourceIds, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default);
}
