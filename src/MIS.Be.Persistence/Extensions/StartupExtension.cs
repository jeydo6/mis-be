using System;
using Microsoft.Extensions.DependencyInjection;
using MIS.Be.Domain.Providers;
using MIS.Be.Domain.Repositories;
using MIS.Be.Persistence.Factories;
using MIS.Be.Persistence.Repositories;

namespace MIS.Be.Persistence.Extensions;

public static class StartupExtension
{
	public static IServiceCollection ConfigureRelease(this IServiceCollection services)
	{
		services
			.AddSingleton<IDateTimeProvider, CurrentDateTimeProvider>();

		services
			.AddScoped<IDbConnectionFactory, SqlConnectionFactory>()
			.AddScoped<IDispanserizationsRepository, DispanserizationsRepository>()
			.AddScoped<IEmployeesRepository, EmployeesRepository>()
			.AddScoped<IPatientsRepository, PatientsRepository>()
			.AddScoped<IResourcesRepository, ResourcesRepository>()
			.AddScoped<IRoomsRepository, RoomsRepository>()
			.AddScoped<ISpecialtiesRepository, SpecialtiesRepository>()
			.AddScoped<ITimeItemsRepository, TimeItemsRepository>()
			.AddScoped<IVisitItemsRepository, VisitItemsRepository>();

		return services;
	}

	public static IServiceCollection ConfigureDebug(this IServiceCollection services)
	{
		services
			.AddSingleton<IDateTimeProvider, DefaultDateTimeProvider>(sp => new DefaultDateTimeProvider(new DateTime(2018, 12, 18)));

		services
			.AddScoped<IDbConnectionFactory, SqlConnectionFactory>()
			.AddScoped<IDispanserizationsRepository, DispanserizationsRepository>()
			.AddScoped<IEmployeesRepository, EmployeesRepository>()
			.AddScoped<IPatientsRepository, PatientsRepository>()
			.AddScoped<IResourcesRepository, ResourcesRepository>()
			.AddScoped<IRoomsRepository, RoomsRepository>()
			.AddScoped<ISpecialtiesRepository, SpecialtiesRepository>()
			.AddScoped<ITimeItemsRepository, TimeItemsRepository>()
			.AddScoped<IVisitItemsRepository, VisitItemsRepository>();

		return services;
	}
}

