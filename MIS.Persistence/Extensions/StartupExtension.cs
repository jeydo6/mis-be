using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;
using MIS.Persistence.Repositories;

namespace MIS.Persistence.Extensions;

public static class StartupExtension
{
	public static IServiceCollection ConfigureRelease(this IServiceCollection services)
	{
		services
			.AddSingleton<IDateTimeProvider, CurrentDateTimeProvider>();

		services
			.AddScoped<IDbConnection, SqlConnection>(sp =>
			{
				var configuration = sp.GetRequiredService<IConfiguration>();

				return new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
			})
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
			.AddScoped<IDbConnection, SqlConnection>(sp =>
			{
				var configuration = sp.GetRequiredService<IConfiguration>();

				return new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
			})
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

