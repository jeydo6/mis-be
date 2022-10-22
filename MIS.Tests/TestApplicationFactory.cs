using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MIS.Application.Startups;

namespace MIS.Tests;

internal sealed class TestApplicationFactory<TStartup>
	where TStartup : StartupBase
{
	private readonly IHostBuilder _builder = CreateHostBuilder();

	public TestApplicationFactory<TStartup> WithHostBuilder(Action<IHostBuilder> configuration)
	{
		configuration(_builder);

		return this;
	}

	public IHost CreateHost() => _builder.Build();

	private static IHostBuilder CreateHostBuilder() => Host
		.CreateDefaultBuilder()
		.ConfigureServices((context, services) =>
		{
			services.AddLogging(l => l.ClearProviders().AddConsole());

			if (Activator.CreateInstance(typeof(TStartup), context.Configuration) is TStartup startup)
			{
				startup.ConfigureServices(services);
			}
		});
}
