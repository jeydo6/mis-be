using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MIS.Application.Configs;
using MIS.Mediator;
using MIS.Persistence.Extensions;

namespace MIS.Tests.Factories;

public class TestApplicationFactory : IApplicationFactory
{
	private IHost _host;

	public IHost CreateHost() => CreateHost(_ => { });

	public IHost CreateHost(Action<IServiceCollection> configuration) =>
		_host ??= Host
			.CreateDefaultBuilder()
			.ConfigureServices((context, services) =>
			{
				services
					.Configure<SettingsConfig>(
						context.Configuration.GetSection($"{nameof(SettingsConfig)}")
					);

				services
					.AddLogging(l => l.ClearProviders().AddConsole())
					.AddMediator(typeof(Application.AssemblyMarker))
					.ConfigureRelease();
			})
			.ConfigureServices(configuration)
			.Build();
}
