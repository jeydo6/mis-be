using System;
using System.Threading;
using System.Threading.Tasks;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories;

public interface IDateItemsRepository
{
    Task<DateItem[]> GetAll(int[] resourceIds, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default);
}
