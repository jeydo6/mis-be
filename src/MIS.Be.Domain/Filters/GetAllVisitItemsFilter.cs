namespace MIS.Be.Domain.Filters;

public sealed record GetAllVisitItemsFilter(
    int? SpecialtyId = default,
    int? ResourceId = default,
    int? PatientId = default
);
