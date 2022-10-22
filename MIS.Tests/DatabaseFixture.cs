using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MIS.Application.Startups;
using Xunit;

namespace MIS.Tests;

public class DatabaseFixture<TStartup> : IClassFixture<TestApplicationFactoryFixture<TStartup>>, ITestApplicationFactoryFixture<TStartup>
	where TStartup : StartupBase
{
	private readonly ITestApplicationFactoryFixture<TStartup> Fixture;

	public IHost CreateHost() =>
	Fixture.CreateHost();

	public IHost CreateHost(Action<IServiceCollection> configuration) =>
		Fixture.CreateHost(configuration);
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection<TStartup> : ICollectionFixture<DatabaseFixture<TStartup>>
	where TStartup : StartupBase
{

}
