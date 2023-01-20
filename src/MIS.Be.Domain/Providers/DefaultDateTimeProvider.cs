using System;

namespace MIS.Be.Domain.Providers
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
