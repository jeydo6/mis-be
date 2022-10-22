using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MIS.Application.Startups;
public abstract class StartupBase
{
	public StartupBase(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	protected IConfiguration Configuration { get; }

	public abstract IServiceCollection ConfigureServices(IServiceCollection services);
}
