using System;

namespace MIS.Be.Domain.Entities;

public sealed class DateItem
{
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
    public int TimeItemsCount { get; set; }
    public int VisitItemsCount { get; set; }
    public int ResourceId { get; set; }
}
