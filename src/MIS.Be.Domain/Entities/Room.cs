namespace MIS.Be.Domain.Entities;

public sealed class Room : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
