namespace MIS.Be.Domain.Entities;

public abstract class BaseEntity
{
	public int Id { get; set; }
    public bool IsActive { get; set; }
}
