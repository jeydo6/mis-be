using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MIS.Application.Startups;

namespace MIS.Tests;

internal sealed class TestApplicationFactory<TStartup>
	where TStartup : StartupBase
{
	public IHostBuilder CreateHostBuilder() => Host
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
