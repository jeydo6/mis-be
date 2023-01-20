using MIS.Be.Tests.Factories;
using Xunit;

namespace MIS.Be.Tests;

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<TestApplicationFactory>
{

}
