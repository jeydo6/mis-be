using System;

namespace MIS.Be.Domain.Entities;

public sealed class TimeItem : BaseEntity
{
    public DateTimeOffset From { get; set; }
	public DateTimeOffset To { get; set; }
	public int ResourceId { get; set; }
}
