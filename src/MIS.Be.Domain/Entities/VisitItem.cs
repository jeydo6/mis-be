
namespace MIS.Be.Domain.Entities;

public sealed class VisitItem : BaseEntity
{
	public int PatientId { get; set; }
	public int TimeItemId { get; set; }
}
