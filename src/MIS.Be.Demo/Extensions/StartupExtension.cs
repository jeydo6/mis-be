using Microsoft.Extensions.DependencyInjection;
using MIS.Be.Demo.DataContexts;
using MIS.Be.Demo.Repositories;
using MIS.Be.Domain.Providers;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Demo.Extensions;

public static class StartupExtension
{
	public static IServiceCollection ConfigureDemo(this IServiceCollection services)
	{
		services
			.AddSingleton<IDateTimeProvider, CurrentDateTimeProvider>();

		services
			.AddSingleton<DemoDataContext>();

		services
			.AddSingleton<IPatientsRepository, PatientsRepository>()
			.AddSingleton<IResourcesRepository, ResourcesRepository>()
			.AddSingleton<ITimeItemsRepository, TimeItemsRepository>()
			.AddSingleton<IVisitItemsRepository, VisitItemsRepository>()
			.AddSingleton<IDispanserizationsRepository, DispanserizationsRepository>();

		return services;
	}
}
