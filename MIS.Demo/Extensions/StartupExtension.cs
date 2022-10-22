using Microsoft.Extensions.DependencyInjection;
using MIS.Demo.DataContexts;
using MIS.Demo.Repositories;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;

namespace MIS.Demo.Extensions;

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
