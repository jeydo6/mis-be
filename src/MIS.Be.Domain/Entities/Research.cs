namespace MIS.Be.Domain.Entities;

public sealed class Research : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsDispanserization { get; set; }
    public int ResourceId { get; set; }
}
