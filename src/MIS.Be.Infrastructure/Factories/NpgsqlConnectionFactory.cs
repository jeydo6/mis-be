using System.Data.Common;
using Npgsql;

namespace MIS.Be.Infrastructure.Factories;

public interface IDbConnectionFactory
{
    DbConnection Create();
}

internal sealed class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly NpgsqlDataSource _dataSource;

    public NpgsqlConnectionFactory(string connectionString)
        => _dataSource = new NpgsqlDataSourceBuilder(connectionString)
            .Build();

    public DbConnection Create()
        => _dataSource.CreateConnection();
}
