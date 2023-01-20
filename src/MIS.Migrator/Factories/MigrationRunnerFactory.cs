using System.IO;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MIS.Migrator.Factories;

public class MigrationRunnerFactory
{
	public static IMigrationRunner CreateMigrationRunner()
	{
		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", false)
			.Build();

		var serviceProvider = new ServiceCollection()
			.AddFluentMigratorCore()
			.AddLogging(builder => builder.AddFluentMigratorConsole())
			.ConfigureRunner(builder => builder
				.AddSqlServer()
				.WithGlobalConnectionString(configuration.GetConnectionString("DefaultConnection"))
				.WithMigrationsIn(typeof(Persistence.AssemblyMarker).Assembly)
			)
			.BuildServiceProvider();

		return serviceProvider.GetRequiredService<IMigrationRunner>();
	}
}
