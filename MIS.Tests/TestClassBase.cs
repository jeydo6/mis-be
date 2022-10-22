﻿using System;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MIS.Application.Startups;
using Xunit;

namespace MIS.Tests;

public abstract class TestClassBase<TStartup> : IClassFixture<DatabaseFixture<TStartup>>
	where TStartup : StartupBase
{
	private readonly ITestApplicationFactoryFixture<TStartup> Fixture;

	protected readonly Faker Faker = new Faker();

	public TestClassBase(DatabaseFixture<TStartup> fixture)
	{
		Fixture = fixture;
	}

	public IHost CreateHost() =>
		Fixture.CreateHost();

	public IHost CreateHost(Action<IServiceCollection> configuration) =>
		Fixture.CreateHost(configuration);
}
