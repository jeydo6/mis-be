namespace MIS.Be.Application.Models;

public record Research(
    int Id,
    string Name,
    bool IsDispanserization,
    int ResourceId
);
