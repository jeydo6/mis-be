using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Be.Domain.Transactions;

public interface ITransactionControl
{
    Task<ITransaction> BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
    Task CommitTransaction(CancellationToken cancellationToken = default);
    Task RollbackTransaction(CancellationToken cancellationToken = default);
}
