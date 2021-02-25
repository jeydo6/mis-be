using MIS.Demo.DataContexts;
using MIS.Domain.Providers;
using System;

namespace MIS.Tests.Fixtures.Demo
{
	public class DataFixture : IDisposable
	{
		public DataFixture()
		{
			DateTimeProvider = new CurrentDateTimeProvider();
			DataContext = new DemoDataContext(DateTimeProvider);
		}

		internal IDateTimeProvider DateTimeProvider { get; }

		internal DemoDataContext DataContext { get; }

		public void Dispose()
		{
			//
		}
	}
}
