namespace MIS.Be.Domain.Filters;

public sealed record GetAllTimeItemsFilter(
    int? SpecialtyId = default,
    int? ResourceId = default
);
