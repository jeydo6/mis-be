using System;
using Bogus;
using Microsoft.Extensions.Configuration;
using MIS.Domain.Providers;

namespace MIS.Tests.Fixtures.Live
{
	public class DataFixture
	{
		public DataFixture()
		{
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
				.Build();

			ConnectionString = configuration.GetConnectionString("DefaultConnection");
			DateTimeProvider = new DefaultDateTimeProvider(new DateTime(2100, 1, 18));
			Faker = new Faker();
		}

		protected internal Faker Faker;

		protected internal IDateTimeProvider DateTimeProvider { get; }

		protected internal string ConnectionString { get; }
	}
}
