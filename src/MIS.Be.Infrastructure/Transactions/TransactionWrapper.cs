using System.Threading;
using System.Threading.Tasks;
using LinqToDB.Data;
using MIS.Be.Domain.Transactions;

namespace MIS.Be.Infrastructure.Transactions;

internal sealed class TransactionWrapper : ITransaction
{
    private readonly DataConnectionTransaction _transaction;

    public TransactionWrapper(DataConnectionTransaction transaction)
        => _transaction = transaction;

    public Task Commit(CancellationToken cancellationToken = default)
        => _transaction.CommitAsync(cancellationToken);

    public Task Rollback(CancellationToken cancellationToken = default)
        => _transaction.RollbackAsync(cancellationToken);

    public void Dispose()
        => _transaction.Dispose();

    public ValueTask DisposeAsync()
        => _transaction.DisposeAsync();
}
