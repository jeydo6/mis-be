using FluentMigrator;

namespace MIS.Persistence.Migrations;

[Migration(0002)]
public class Migration_0002 : Migration
{
	public override void Up()
	{
		Execute.Script("./Tables/Specialties.Up.sql");
		Execute.Script("./Tables/Employees.Up.sql");
		Execute.Script("./Tables/Rooms.Up.sql");
		Execute.Script("./Tables/Resources.Up.sql");
		Execute.Script("./Tables/Patients.Up.sql");
		Execute.Script("./Tables/TimeItems.Up.sql");
		Execute.Script("./Tables/VisitItems.Up.sql");
		Execute.Script("./Tables/Dispanserizations.Up.sql");
	}

	public override void Down()
	{
		Execute.Script("./Tables/Dispanserizations.Down.sql");
		Execute.Script("./Tables/VisitItems.Down.sql");
		Execute.Script("./Tables/TimeItems.Down.sql");
		Execute.Script("./Tables/Patients.Down.sql");
		Execute.Script("./Tables/Resources.Down.sql");
		Execute.Script("./Tables/Rooms.Down.sql");
		Execute.Script("./Tables/Employees.Down.sql");
		Execute.Script("./Tables/Specialties.Down.sql");
	}
}
