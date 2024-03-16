namespace MIS.Be.Domain.Filters;

public sealed record GetAllResourcesFilter(
    int? SpecialtyId = default
);
