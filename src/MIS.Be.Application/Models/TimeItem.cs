using System;

namespace MIS.Be.Application.Models;

public sealed record TimeItem(int Id, DateTimeOffset From, DateTimeOffset To, int ResourceId);
