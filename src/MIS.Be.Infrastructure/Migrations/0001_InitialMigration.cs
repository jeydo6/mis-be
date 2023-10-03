using FluentMigrator;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Infrastructure.Migrations;

[Migration(0001)]
public sealed class InitialMigration : Migration
{
    public override void Up()
    {
        Create.Table(nameof(Patient))
            .WithColumn(nameof(Patient.Id)).AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn(nameof(Patient.IsActive)).AsBoolean().NotNullable()
            .WithColumn(nameof(Patient.Code)).AsString().NotNullable()
            .WithColumn(nameof(Patient.FirstName)).AsString().NotNullable()
            .WithColumn(nameof(Patient.MiddleName)).AsString().NotNullable()
            .WithColumn(nameof(Patient.LastName)).AsString().NotNullable()
            .WithColumn(nameof(Patient.BirthDate)).AsDate().NotNullable()
            .WithColumn(nameof(Patient.Gender)).AsInt32().NotNullable();

        Create.Table(nameof(Dispanserization))
            .WithColumn(nameof(Dispanserization.Id)).AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn(nameof(Dispanserization.IsActive)).AsBoolean().NotNullable()
            .WithColumn(nameof(Dispanserization.From)).AsDate().NotNullable()
            .WithColumn(nameof(Dispanserization.To)).AsDate().NotNullable()
            .WithColumn(nameof(Dispanserization.PatientId)).AsInt32().NotNullable().ForeignKey(nameof(Patient), nameof(Patient.Id));

        Create.Table(nameof(Employee))
            .WithColumn(nameof(Employee.Id)).AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn(nameof(Employee.IsActive)).AsBoolean().NotNullable()
            .WithColumn(nameof(Employee.Code)).AsString().NotNullable()
            .WithColumn(nameof(Employee.FirstName)).AsString().NotNullable()
            .WithColumn(nameof(Employee.MiddleName)).AsString().NotNullable()
            .WithColumn(nameof(Employee.LastName)).AsString().NotNullable();

        Create.Table(nameof(Room))
            .WithColumn(nameof(Room.Id)).AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn(nameof(Room.IsActive)).AsBoolean().NotNullable()
            .WithColumn(nameof(Room.Code)).AsString().NotNullable()
            .WithColumn(nameof(Room.Description)).AsString().NotNullable();

        Create.Table(nameof(Specialty))
            .WithColumn(nameof(Specialty.Id)).AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn(nameof(Specialty.IsActive)).AsBoolean().NotNullable()
            .WithColumn(nameof(Specialty.Code)).AsString().NotNullable()
            .WithColumn(nameof(Specialty.Name)).AsString().NotNullable();

        Create.Table(nameof(Resource))
            .WithColumn(nameof(Resource.Id)).AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn(nameof(Resource.IsActive)).AsBoolean().NotNullable()
            .WithColumn(nameof(Resource.Name)).AsString().NotNullable()
            .WithColumn(nameof(Resource.Type)).AsInt32().NotNullable()
            .WithColumn(nameof(Resource.IsDispanserization)).AsBoolean().NotNullable()
            .WithColumn(nameof(Resource.EmployeeId)).AsInt32().NotNullable().ForeignKey(nameof(Employee), nameof(Employee.Id))
            .WithColumn(nameof(Resource.RoomId)).AsInt32().NotNullable().ForeignKey(nameof(Room), nameof(Room.Id))
            .WithColumn(nameof(Resource.SpecialtyId)).AsInt32().NotNullable().ForeignKey(nameof(Specialty), nameof(Specialty.Id));

        Create.Table(nameof(Research))
            .WithColumn(nameof(Research.Id)).AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn(nameof(Research.IsActive)).AsBoolean().NotNullable()
            .WithColumn(nameof(Research.Description)).AsString().NotNullable()
            .WithColumn(nameof(Research.IsDispanserization)).AsBoolean().NotNullable()
            .WithColumn(nameof(Research.ResourceId)).AsInt32().NotNullable().ForeignKey(nameof(Resource), nameof(Resource.Id));

        Create.Table(nameof(TimeItem))
            .WithColumn(nameof(TimeItem.Id)).AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn(nameof(TimeItem.IsActive)).AsBoolean().NotNullable()
            .WithColumn(nameof(TimeItem.From)).AsDateTimeOffset().NotNullable()
            .WithColumn(nameof(TimeItem.To)).AsDateTimeOffset().NotNullable()
            .WithColumn(nameof(TimeItem.ResourceId)).AsInt32().NotNullable().ForeignKey(nameof(Resource), nameof(Resource.Id));

        Create.Table(nameof(VisitItem))
            .WithColumn(nameof(VisitItem.Id)).AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn(nameof(VisitItem.IsActive)).AsBoolean().NotNullable()
            .WithColumn(nameof(VisitItem.PatientId)).AsInt32().NotNullable().ForeignKey(nameof(Patient), nameof(Patient.Id))
            .WithColumn(nameof(VisitItem.TimeItemId)).AsInt32().NotNullable().ForeignKey(nameof(TimeItem), nameof(TimeItem.Id));

        Create.UniqueConstraint()
            .OnTable(nameof(Patient))
            .Column(nameof(Patient.Code));

        Create.UniqueConstraint()
            .OnTable(nameof(Employee))
            .Column(nameof(Employee.Code));

        Create.UniqueConstraint()
            .OnTable(nameof(Room))
            .Column(nameof(Room.Code));

        Create.UniqueConstraint()
            .OnTable(nameof(Specialty))
            .Column(nameof(Specialty.Code));

        Create.UniqueConstraint()
            .OnTable(nameof(VisitItem))
            .Column(nameof(VisitItem.TimeItemId));
    }

    public override void Down()
    {
        Delete.Table(nameof(Patient));
        Delete.Table(nameof(Dispanserization));
        Delete.Table(nameof(Employee));
        Delete.Table(nameof(Room));
        Delete.Table(nameof(Specialty));
        Delete.Table(nameof(Resource));
        Delete.Table(nameof(Research));
        Delete.Table(nameof(TimeItem));
        Delete.Table(nameof(VisitItem));
    }
}
