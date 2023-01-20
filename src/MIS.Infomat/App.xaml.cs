using System;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MIS.Infomat.Windows;

namespace MIS.Infomat
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : System.Windows.Application
	{
		public App()
		{
			var host = Program.CreateHost();
			if (host is null || host.Services is null)
			{
				Shutdown();
			}

			ServiceProvider = host.Services;
		}

		public IServiceProvider ServiceProvider { get; }

		private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			var logger = ServiceProvider.GetRequiredService<ILogger<App>>();

			logger.LogError(e.Exception, "Unhandled exception of type '{type}' was thrown.", e.Exception.GetType());
			e.Handled = true;

			MainWindow mainWindow = Current.MainWindow as MainWindow;
			mainWindow.MainWorkflow();
		}
	}
}
