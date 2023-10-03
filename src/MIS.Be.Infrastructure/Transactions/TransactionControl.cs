using System.Data;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB.Data;
using MIS.Be.Domain.Transactions;

namespace MIS.Be.Infrastructure.Transactions;

internal sealed class TransactionControl<T> : ITransactionControl where T : DataConnection
{
    private readonly T _db;

    public TransactionControl(T db) => _db = db;

    public async Task<ITransaction> BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
    {
        var transaction = await _db.BeginTransactionAsync(isolationLevel, cancellationToken);

        return new TransactionWrapper(transaction);
    }

    public Task CommitTransaction(CancellationToken cancellationToken = default)
        => _db.CommitTransactionAsync(cancellationToken);

    public Task RollbackTransaction(CancellationToken cancellationToken = default)
        => _db.RollbackTransactionAsync(cancellationToken);
}
