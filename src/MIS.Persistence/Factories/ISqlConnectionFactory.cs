using System.Data;

namespace MIS.Persistence.Factories;

public interface IDbConnectionFactory
{
	IDbConnection CreateConnection();
}
