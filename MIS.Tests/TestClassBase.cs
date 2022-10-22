using System;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MIS.Application.Startups;
using Xunit;

namespace MIS.Tests;

public abstract class TestClassBase<T> : IClassFixture<TestApplicationFactoryFixture<T>>, ITestApplicationFactoryFixture<T>
	where T : StartupBase
{
	private readonly ITestApplicationFactoryFixture<T> Fixture;

	protected readonly Faker Faker = new Faker();

	public TestClassBase(TestApplicationFactoryFixture<T> fixture)
	{
		Fixture = fixture;
	}

	public IHost CreateHost() =>
		Fixture.CreateHost();

	public IHost CreateHost(Action<IServiceCollection> configuration) =>
		Fixture.CreateHost(configuration);
}
