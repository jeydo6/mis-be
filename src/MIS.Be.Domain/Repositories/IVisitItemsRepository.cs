using System;
using System.Threading;
using System.Threading.Tasks;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories;

public interface IVisitItemsRepository
{
    Task<int> Create(VisitItem item, CancellationToken cancellationToken = default);
    Task<VisitItem> Get(int id, CancellationToken cancellationToken = default);
    Task<VisitItem[]> GetAll(int patientId, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default);
}
