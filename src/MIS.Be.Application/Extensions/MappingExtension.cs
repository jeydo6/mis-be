using MIS.Be.Domain.Extensions;

namespace MIS.Be.Application.Extensions;

internal static class MappingExtension
{
    public static Models.Dispanserization Map(this Domain.Entities.Dispanserization source)
        => new Models.Dispanserization(
            source.Id,
            source.IsCompleted,
            source.From,
            source.To,
            source.PatientId);

    public static Models.Employee Map(this Domain.Entities.Employee source)
        => new Models.Employee(
            source.Id,
            source.Code,
            source.GetName()
        );

    public static Models.Patient Map(this Domain.Entities.Patient source)
        => new Models.Patient(
            source.Id,
            source.Code,
            source.GetName()
        );

    public static Models.Research Map(this Domain.Entities.Research source)
        => new Models.Research(
            source.Id,
            source.Name,
            source.IsDispanserization,
            source.ResourceId
        );

    public static Models.Resource Map(this Domain.Entities.Resource source)
        => new Models.Resource(
            source.Id,
            source.Name,
            source.Type,
            source.IsDispanserization,
            source.EmployeeId,
            source.RoomId,
            source.SpecialtyId
        );

    public static Models.Room Map(this Domain.Entities.Room source)
        => new Models.Room(
            source.Id,
            source.Code,
            source.Description
        );

    public static Models.Specialty Map(this Domain.Entities.Specialty source)
        => new Models.Specialty(
            source.Id,
            source.Code,
            source.Name
        );

    public static Models.DateItem Map(this Domain.Entities.DateItem source)
        => new Models.DateItem(
            source.From,
            source.To,
            source.TimeItemsCount - source.VisitItemsCount,
            source.ResourceId
        );

    public static Models.TimeItem Map(this Domain.Entities.TimeItem source)
        => new Models.TimeItem(
            source.Id,
            source.From,
            source.To,
            source.ResourceId
        );

    public static Models.VisitItem Map(this Domain.Entities.VisitItem source)
        => new Models.VisitItem(
            source.Id,
            source.PatientId,
            source.TimeItemId
        );
}
