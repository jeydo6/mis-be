namespace MIS.Be.Application.Models;

public sealed record Specialty(
    int Id,
    string Code,
    string Name,
    bool IsActive
);
