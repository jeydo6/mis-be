using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MIS.Tests;

internal interface ITestApplicationFactoryFixture
{
	IHost CreateHost() => CreateHost(_ => { });

	IHost CreateHost(Action<IServiceCollection> configuration);
}

public class TestApplicationFactoryFixture : ITestApplicationFactoryFixture
{
	private readonly TestApplicationFactory _factory = new TestApplicationReleaseFactory();

	public IHost CreateHost(Action<IServiceCollection> configuration) =>
		_factory
			.WithHostBuilder(builder => builder.ConfigureServices(configuration))
			.Build();
}
