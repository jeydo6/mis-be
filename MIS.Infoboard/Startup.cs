using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.Configs;
using MIS.Mediator;
using MIS.Persistence.Extensions;

namespace MIS.Infoboard;

internal sealed class Startup
{
	public Startup(IConfiguration configuration) =>
		Configuration = configuration;

	private IConfiguration Configuration { get; }

	public IServiceCollection ConfigureServices(IServiceCollection services)
	{
		services.Configure<SettingsConfig>(
			Configuration.GetSection($"{nameof(SettingsConfig)}")
		);

		services.Configure<ContactsConfig>(
			Configuration.GetSection($"{nameof(ContactsConfig)}")
		);

		services
			.AddMediator(typeof(Application.AssemblyMarker));

#if DEMO
		services
			.ConfigureDemo();
#elif DEBUG
		services
			.ConfigureDebug();
#elif RELEASE
		services
			.ConfigureRelease(options =>
			{
				options.ConnectionString = Configuration
					.GetConnectionString("DefaultConnection");
			});
#else
		throw new ApplicationException("Unknown project configuration!");
#endif

		return services;
	}
}
