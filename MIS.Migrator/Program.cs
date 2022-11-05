using CommandLine;
using MIS.Migrator.Factories;

namespace MIS.Migrator;

internal class Program
{
	private static void Main(string[] args)
	{
		new Parser(o => o.AutoHelp = false)
			.ParseArguments<MigratorArgs>(args)
			.WithParsed(o =>
			{
				var migrationRunner = MigrationRunnerFactory.CreateMigrationRunner();

				if (o.Down > 0 && migrationRunner.HasMigrationsToApplyDown(o.Down))
					migrationRunner.MigrateDown(o.Down);

				if (migrationRunner.HasMigrationsToApplyUp())
					migrationRunner.MigrateUp();
			});
	}
}
