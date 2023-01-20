using FluentMigrator;

namespace MIS.Be.Persistence.Migrations;

[Migration(0002)]
public class Migration_0002 : Migration
{
	public override void Up()
	{
		Execute.Script("./StoredProcedures/sp_Dispanserizations_Create.Up.sql");
		Execute.Script("./StoredProcedures/sp_Dispanserizations_Get.Up.sql");
		Execute.Script("./StoredProcedures/sp_Dispanserizations_List.Up.sql");
		Execute.Script("./StoredProcedures/sp_Employees_Create.Up.sql");
		Execute.Script("./StoredProcedures/sp_Employees_Get.Up.sql");
		Execute.Script("./StoredProcedures/sp_Patients_Create.Up.sql");
		Execute.Script("./StoredProcedures/sp_Patients_Find.Up.sql");
		Execute.Script("./StoredProcedures/sp_Patients_Get.Up.sql");
		Execute.Script("./StoredProcedures/sp_Resources_Create.Up.sql");
		Execute.Script("./StoredProcedures/sp_Resources_Get.Up.sql");
		Execute.Script("./StoredProcedures/sp_Resources_GetDispanserizations.Up.sql");
		Execute.Script("./StoredProcedures/sp_Resources_List.Up.sql");
		Execute.Script("./StoredProcedures/sp_Rooms_Create.Up.sql");
		Execute.Script("./StoredProcedures/sp_Rooms_Get.Up.sql");
		Execute.Script("./StoredProcedures/sp_Specialties_Create.Up.sql");
		Execute.Script("./StoredProcedures/sp_Specialties_Get.Up.sql");
		Execute.Script("./StoredProcedures/sp_TimeItems_Create.Up.sql");
		Execute.Script("./StoredProcedures/sp_TimeItems_Get.Up.sql");
		Execute.Script("./StoredProcedures/sp_TimeItems_GetDispanserizationTotals.Up.sql");
		Execute.Script("./StoredProcedures/sp_TimeItems_GetResourceTotals.Up.sql");
		Execute.Script("./StoredProcedures/sp_TimeItems_List.Up.sql");
		Execute.Script("./StoredProcedures/sp_VisitItems_Create.Up.sql");
		Execute.Script("./StoredProcedures/sp_VisitItems_Get.Up.sql");
		Execute.Script("./StoredProcedures/sp_VisitItems_List.Up.sql");
	}

	public override void Down()
	{
		Execute.Script("./StoredProcedures/sp_Dispanserizations_Create.Down.sql");
		Execute.Script("./StoredProcedures/sp_Dispanserizations_Get.Down.sql");
		Execute.Script("./StoredProcedures/sp_Dispanserizations_List.Down.sql");
		Execute.Script("./StoredProcedures/sp_Employees_Create.Down.sql");
		Execute.Script("./StoredProcedures/sp_Employees_Get.Down.sql");
		Execute.Script("./StoredProcedures/sp_Patients_Create.Down.sql");
		Execute.Script("./StoredProcedures/sp_Patients_Find.Down.sql");
		Execute.Script("./StoredProcedures/sp_Patients_Get.Down.sql");
		Execute.Script("./StoredProcedures/sp_Resources_Create.Down.sql");
		Execute.Script("./StoredProcedures/sp_Resources_Get.Down.sql");
		Execute.Script("./StoredProcedures/sp_Resources_GetDispanserizations.Down.sql");
		Execute.Script("./StoredProcedures/sp_Resources_List.Down.sql");
		Execute.Script("./StoredProcedures/sp_Rooms_Create.Down.sql");
		Execute.Script("./StoredProcedures/sp_Rooms_Get.Down.sql");
		Execute.Script("./StoredProcedures/sp_Specialties_Create.Down.sql");
		Execute.Script("./StoredProcedures/sp_Specialties_Get.Down.sql");
		Execute.Script("./StoredProcedures/sp_TimeItems_Create.Down.sql");
		Execute.Script("./StoredProcedures/sp_TimeItems_Get.Down.sql");
		Execute.Script("./StoredProcedures/sp_TimeItems_GetDispanserizationTotals.Down.sql");
		Execute.Script("./StoredProcedures/sp_TimeItems_GetResourceTotals.Down.sql");
		Execute.Script("./StoredProcedures/sp_TimeItems_List.Down.sql");
		Execute.Script("./StoredProcedures/sp_VisitItems_Create.Down.sql");
		Execute.Script("./StoredProcedures/sp_VisitItems_Get.Down.sql");
		Execute.Script("./StoredProcedures/sp_VisitItems_List.Down.sql");
	}
}
