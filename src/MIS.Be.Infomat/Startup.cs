using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MIS.Be.Application.Configs;
using MIS.Be.Domain.Services;
using MIS.Be.Infomat.Services;
using MIS.Be.Mediator;
using MIS.Be.Persistence.Extensions;

namespace MIS.Be.Infomat
{
	internal sealed class Startup
	{
		public Startup(IConfiguration configuration) =>
			Configuration = configuration;

		public IConfiguration Configuration { get; }

		public IServiceCollection ConfigureServices(IServiceCollection services)
		{
			services.Configure<SettingsConfig>(
				Configuration.GetSection($"{nameof(SettingsConfig)}")
			);

			services
				.AddMediator(typeof(Application.AssemblyMarker));

			services
				.AddSingleton<IPrintService, XPSPrintService>();

#if DEMO
			services
				.ConfigureDemo();
#elif DEBUG
			services
				.ConfigureDebug();
#elif RELEASE
			services
				.ConfigureRelease();
#else
			throw new Exception("Unknown project configuration!");
#endif

			return services;
		}
	}
}
