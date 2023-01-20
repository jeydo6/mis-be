using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MIS.Be.Application.Configs;
using MIS.Be.Mediator;
using MIS.Be.Migrator.Factories;
using MIS.Be.Persistence.Extensions;

namespace MIS.Be.Tests.Factories;

public class TestApplicationFactory : IApplicationFactory, IDisposable
{
	private IHost _host;

	public TestApplicationFactory()
	{
		var migrationRunner = MigrationRunnerFactory.CreateMigrationRunner();
		//if (migrationRunner.HasMigrationsToApplyDown(0))
		//	migrationRunner.MigrateDown(0);

		if (migrationRunner.HasMigrationsToApplyUp())
			migrationRunner.MigrateUp();
	}

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

	public void Dispose() => _host?.Dispose();
}
