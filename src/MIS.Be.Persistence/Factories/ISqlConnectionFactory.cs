using System.Data;

namespace MIS.Be.Persistence.Factories;

public interface IDbConnectionFactory
{
	IDbConnection CreateConnection();
}
