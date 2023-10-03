using MIS.Be.Domain.Enums;

namespace MIS.Be.Domain.Entities;

public sealed class Resource : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ResourceType Type { get; set; }
    public bool IsDispanserization { get; set; }
    public int EmployeeId { get; set; }
    public int RoomId { get; set; }
    public int SpecialtyId { get; set; }
}
