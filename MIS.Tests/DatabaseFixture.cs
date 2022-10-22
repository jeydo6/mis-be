using MIS.Application.Startups;
using MIS.Infomat;
using Xunit;

namespace MIS.Tests;

public class DatabaseFixture<TStartup> : TestApplicationFactoryFixture<TStartup>
	where TStartup : StartupBase
{

}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture<Startup>>
{

}
