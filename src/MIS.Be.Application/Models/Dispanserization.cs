using System;

namespace MIS.Be.Application.Models;

public sealed record Dispanserization(int Id, bool IsActive, DateTimeOffset From, DateTimeOffset To, int PatientId);
