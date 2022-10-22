using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MIS.Application.Startups;

namespace MIS.Tests;

internal sealed class TestApplicationFactory<TStartup>
	where TStartup : StartupBase
{
	//public static IHost WithHostBuilder(Action<IHostBuilder> configuration)
	//{
	//	var builder = CreateHostBuilder();
	//	configuration(builder);

	//	return builder
	//		.Build();
	//}

	//public static IHost CreateHost(Action<IServiceCollection> configuration) =>
	//	CreateHostBuilder()
	//		.ConfigureServices(configuration)
	//		.Build();

	//private IHost _host;

	//public IServiceProvider Services => _host.Services;

	//public TestApplicationFactory<TStartup> WithHostBuilder(Action<IHostBuilder> configuration)
	//{
	//	var builder = CreateHostBuilder();
	//	configuration(builder);

	//	_host = builder.Build();

	//	return this;
	//}

	//private static IHostBuilder CreateHostBuilder() =>
	//	Host
	//		.CreateDefaultBuilder()
	//		.ConfigureServices((context, services) =>
	//		{
	//			services.AddLogging(l => l.ClearProviders().AddConsole());

	//			if (Activator.CreateInstance(typeof(TStartup), context.Configuration) is TStartup startup)
	//			{
	//				startup.ConfigureServices(services);
	//			}
	//		});

	//public void Dispose() => _host?.Dispose();

	public static IHost WithHostBuilder(Action<IHostBuilder> configuration)
	{
		var builder = CreateHostBuilder();
		configuration(builder);

		return builder.Build();
	}

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
