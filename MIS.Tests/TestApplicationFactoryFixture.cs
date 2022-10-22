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

public class TestApplicationFactoryFixture<TStartup> : ITestApplicationFactoryFixture<TStartup>
	where TStartup : StartupBase
{
	private readonly TestApplicationFactory<TStartup> _factory = new TestApplicationFactory<TStartup>();

	public IHost CreateHost(Action<IServiceCollection> configuration) =>
		_factory
			.WithHostBuilder(builder => builder.ConfigureServices(configuration))
			.CreateHost();
}
