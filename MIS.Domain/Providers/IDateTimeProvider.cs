using System;

namespace MIS.Domain.Providers
{
	public interface IDateTimeProvider
	{
		DateTime Now { get; }
	}
}
