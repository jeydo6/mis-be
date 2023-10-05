using System;
using MIS.Be.Domain.Enums;

namespace MIS.Be.Domain.Entities;

public sealed class Patient : Person
{
    public DateTimeOffset BirthDate { get; set; }
    public Gender Gender { get; set; }
}