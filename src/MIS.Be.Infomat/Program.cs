using Microsoft.Extensions.Hosting;
using Serilog;

namespace MIS.Be.Infomat
{
	internal static class Program
	{
		public static IHost CreateHost() =>
			CreateHostBuilder()
				.Build();

		private static IHostBuilder CreateHostBuilder() =>
			Host
				.CreateDefaultBuilder()
				.UseSerilog((context, loggerConfiguration) =>
				{
					loggerConfiguration
						.ReadFrom.Configuration(context.Configuration);
				})
				.ConfigureServices((context, services) =>
				{
					new Startup(context.Configuration)
						.ConfigureServices(services);
				});
	}
}
