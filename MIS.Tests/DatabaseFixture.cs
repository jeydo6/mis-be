using Xunit;

namespace MIS.Tests;

public class DatabaseFixture : TestApplicationFactoryFixture
{

}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{

}
