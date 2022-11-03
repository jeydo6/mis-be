using MIS.Tests.Factories;
using Xunit;

namespace MIS.Tests;

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<TestApplicationFactory>
{

}
