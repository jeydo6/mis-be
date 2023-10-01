using System;

namespace MIS.Be.Application.Models;

public sealed record Dispanserization(int Id, DateOnly From, DateOnly To, bool IsActive, int PatientId);
