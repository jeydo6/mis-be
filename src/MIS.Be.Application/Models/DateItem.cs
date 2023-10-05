using System;

namespace MIS.Be.Application.Models;

public sealed record DateItem(DateTimeOffset From, DateTimeOffset To, int Count, int ResourceId);
