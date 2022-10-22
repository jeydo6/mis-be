using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MIS.Application.Startups;

namespace MIS.Tests;

internal interface ITestApplicationFactoryFixture<TStartup>
	where TStartup : StartupBase
{
	IHost CreateHost() => CreateHost(_ => { });

	IHost CreateHost(Action<IServiceCollection> configuration);
}

public sealed class TestApplicationFactoryFixture<TStartup> : ITestApplicationFactoryFixture<TStartup>
	where TStartup : StartupBase
{
	public IHost CreateHost(Action<IServiceCollection> configuration) =>
		TestApplicationFactory<TStartup>
			.WithHostBuilder(builder => builder.ConfigureServices(configuration));
}
