using System;

namespace MIS.Be.Domain.Entities;

public sealed class Dispanserization : BaseEntity
{
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
    public int PatientId { get; set; }
}
