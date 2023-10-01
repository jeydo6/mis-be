namespace MIS.Be.Domain.Entities;

public sealed class Research : BaseEntity
{
	public string Description { get; set; } = string.Empty;
    public bool IsDispanserization { get; set; }
	public int ResourceId { get; set; }
}
