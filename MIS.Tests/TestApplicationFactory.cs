using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MIS.Application.Configs;
using MIS.Mediator;
using MIS.Persistence.Extensions;

namespace MIS.Tests;

abstract class TestApplicationFactory
{
	public IHostBuilder WithHostBuilder(Action<IHostBuilder> configuration)
	{
		var builder = CreateHostBuilder();
		configuration(builder);

		return builder;
	}

	protected abstract IHostBuilder CreateHostBuilder();
}


internal sealed class TestApplicationReleaseFactory : TestApplicationFactory
{
	protected override IHostBuilder CreateHostBuilder() => Host
		.CreateDefaultBuilder()
		.ConfigureServices((context, services) =>
		{
			services
				.Configure<SettingsConfig>(
					context.Configuration.GetSection($"{nameof(SettingsConfig)}")
				);

			services
				.AddLogging(l => l.ClearProviders().AddConsole())
				.AddMediator()
				.ConfigureRelease();
		});
}