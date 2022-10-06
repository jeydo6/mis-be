using System.Data;
using Microsoft.Data.SqlClient;

namespace MIS.Persistence.Repositories;

public class BaseRepository
{
	private readonly string _connectionString;

	public BaseRepository(string connectionString) =>
		_connectionString = connectionString;

	protected IDbConnection OpenConnection()
	{
		var db = new SqlConnection(_connectionString);
		db.Open();

		return db;
	}
}
