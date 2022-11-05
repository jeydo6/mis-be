using CommandLine;

namespace MIS.Migrator;

[Verb("migrate", HelpText = "Perform database migration")]
internal class MigratorArgs
{
	[Option('d', "down", HelpText = "Perform migrate down to specified version")]
	public long Down { get; set; }
}
