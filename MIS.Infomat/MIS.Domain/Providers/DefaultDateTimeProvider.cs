using System;

namespace MIS.Domain.Providers
{
    public class DefaultDateTimeProvider : IDateTimeProvider
    {
        public DefaultDateTimeProvider(DateTime now)
        {
            Now = now;
        }

        public DateTime Now { get; }
    }
}
