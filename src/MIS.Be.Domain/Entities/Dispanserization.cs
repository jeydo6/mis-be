using System;

namespace MIS.Be.Domain.Entities;

public sealed class Dispanserization : BaseEntity
{
    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    public int PatientId { get; set; }
}
