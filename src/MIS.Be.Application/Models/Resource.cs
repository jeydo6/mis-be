using MIS.Be.Domain.Enums;

namespace MIS.Be.Application.Models;

public sealed record Resource(
    int Id,
    string Name,
    ResourceType Type,
    bool IsDispanserization,
    int EmployeeId,
    int RoomId,
    int SpecialtyId
);
