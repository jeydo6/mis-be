using System;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MIS.Infoboard.Windows;
using Serilog;

namespace MIS.Infoboard;

internal class App : Avalonia.Application
{
	private readonly IHost _host;

	public App()
	{
		_host = CreateHostBuilder()
			.Build();
	}

	public IServiceProvider ServiceProvider => _host.Services;

	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.MainWindow = new MainWindow();
		}

		base.OnFrameworkInitializationCompleted();
	}

	private static IHostBuilder CreateHostBuilder() =>
		Host
			.CreateDefaultBuilder()
			.ConfigureAppConfiguration(context =>
				context.AddJsonFile("contacts.json", optional: true)
			)
			.UseSerilog((context, loggerConfiguration) =>
				loggerConfiguration
					.ReadFrom.Configuration(context.Configuration)
			)
			.ConfigureServices((context, services) =>
				new Startup(context.Configuration)
					.ConfigureServices(services)
			);
}

internal static class ApplicationExtension
{
	public static IServiceProvider GetServiceProvider(this Avalonia.Application? application) =>
		application is App app ?
			app.ServiceProvider :
			throw new ApplicationException("Unknown type of the Application");
}
