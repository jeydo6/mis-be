using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MIS.Be.Domain.Repositories;
using MIS.Be.Infrastructure.DataContexts;
using MIS.Be.Infrastructure.Repositories;

namespace MIS.Be.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
	private const string ConnectionStringName = "DefaultConnection";

	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString(ConnectionStringName);
		ArgumentException.ThrowIfNullOrEmpty(connectionString);

		services
			.AddFluentMigratorCore()
			.AddLogging(builder => builder.AddFluentMigratorConsole())
			.ConfigureRunner(builder => builder
				.AddPostgres()
				.WithGlobalConnectionString(connectionString)
				.ScanIn(typeof(DbContext).Assembly).For.Migrations());

		services
			.AddScoped<DbContext>(_ => new DbContext(connectionString))
            .AddScoped<IDispanserizationsRepository, DispanserizationsRepository>()
            .AddScoped<IEmployeesRepository, EmployeesRepository>()
            .AddScoped<IPatientsRepository, PatientsRepository>()
            .AddScoped<IResearchesRepository, ResearchesRepository>()
            .AddScoped<IResourcesRepository, ResourcesRepository>()
            .AddScoped<IRoomsRepository, RoomsRepository>()
            .AddScoped<ISpecialtiesRepository, SpecialtiesRepository>()
            .AddScoped<ITimeItemsRepository, TimeItemsRepository>()
            .AddScoped<IVisitItemsRepository, VisitItemsRepository>();
	}
}
