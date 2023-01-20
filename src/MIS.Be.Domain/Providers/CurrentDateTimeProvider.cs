using System;

namespace MIS.Be.Domain.Providers
{
	public class CurrentDateTimeProvider : IDateTimeProvider
	{
		public DateTime Now
		{
			get
			{
				return DateTime.Now;
			}
		}
	}
}
