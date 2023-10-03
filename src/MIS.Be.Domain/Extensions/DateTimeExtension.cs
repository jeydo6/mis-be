using System;

namespace MIS.Be.Domain.Extensions;

public static class DateTimeExtension
{
    public static DateTimeOffset ToDateTimeOffset(this DateOnly date, TimeOnly time)
        => new DateTimeOffset(
            date.Year, date.Month, date.Day,
            time.Hour, time.Minute, time.Second, time.Millisecond, time.Microsecond,
            TimeSpan.Zero);

    public static DateTimeOffset ToDateTimeOffset(this DateOnly date, TimeOnly time, TimeSpan offset)
        => new DateTimeOffset(
            date.Year, date.Month, date.Day,
            time.Hour, time.Minute, time.Second, time.Millisecond, time.Microsecond,
            offset);
}
