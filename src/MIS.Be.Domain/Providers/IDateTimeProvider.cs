using System;

namespace MIS.Be.Domain.Providers
{
	public interface IDateTimeProvider
	{
		DateTime Now { get; }
	}
}
