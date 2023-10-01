namespace MIS.Be.Domain.Entities;

public sealed class Specialty : BaseEntity
{
	public string Code { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
}
