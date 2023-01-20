using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MIS.Be.Persistence.Factories;

public sealed class SqlConnectionFactory : IDbConnectionFactory
{
	private readonly IConfiguration _configuration;

	public SqlConnectionFactory(IConfiguration configuration) =>
		_configuration = configuration;

	public IDbConnection CreateConnection() =>
		new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
}
