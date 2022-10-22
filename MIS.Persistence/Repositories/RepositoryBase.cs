using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MIS.Persistence.Repositories;

public class RepositoryBase
{
	private const string DefaultConnection = "DefaultConnection";

	private readonly string _connectionString;

	public RepositoryBase(IConfiguration configuration) =>
		_connectionString = configuration.GetConnectionString(DefaultConnection);

	protected IDbConnection OpenConnection()
	{
		var db = new SqlConnection(_connectionString);
		db.Open();

		return db;
	}
}
