using System;
using MIS.Demo.DataContexts;
using MIS.Domain.Providers;

namespace MIS.Tests.Fixtures.Demo
{
	public class DataFixture : IDisposable
	{
		public DataFixture()
		{
			DataContext = new DemoDataContext(DateTimeProvider);
			DateTimeProvider = new CurrentDateTimeProvider();
		}

		protected internal IDateTimeProvider DateTimeProvider { get; }

		protected internal DemoDataContext DataContext { get; }

		public void Dispose()
		{
			//
		}
	}
}
