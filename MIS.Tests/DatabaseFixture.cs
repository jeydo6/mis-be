using Xunit;

namespace MIS.Tests;

public class DatabaseFixture
{

}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{

}
