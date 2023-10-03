using System;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Be.Domain.Transactions;

public interface ITransaction : IDisposable, IAsyncDisposable
{
    Task Commit(CancellationToken cancellationToken = default);
    Task Rollback(CancellationToken cancellationToken = default);
}
