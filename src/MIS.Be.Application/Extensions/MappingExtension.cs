using MIS.Be.Domain.Extensions;

namespace MIS.Be.Application.Extensions;

internal static class MappingExtension
{
    public static Models.Dispanserization Map(this Domain.Entities.Dispanserization source)
        => new Models.Dispanserization(
            source.Id,
            source.From,
            source.To,
            source.IsActive,
            source.PatientId);

    public static Models.Patient Map(this Domain.Entities.Patient source)
        => new Models.Patient(
            source.Id,
            source.Code,
            source.GetName()
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

    public static Models.Specialty Map(this Domain.Entities.Specialty source)
        => new Models.Specialty(
            source.Id,
            source.Code,
            source.Name
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
