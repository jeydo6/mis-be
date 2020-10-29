using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;
using MIS.Infomat.Services;
using System;
using MIS.Demo.DataContexts;
using MediatR;
using MIS.Domain.Services;
using MIS.Domain.Configs;

namespace MIS.Infomat
{
    using Demo = Demo.Repositories;
    using Live = Persistence.Repositories;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.Configure<ServiceConfig>(
                Configuration.GetSection($"{nameof(ServiceConfig)}")
            );

            services
                .AddMediatR(typeof(Application.AssemblyMarker));

#if DEMO
            ConfigureDemo(services);
#else
            ConfigureLive(services);
#endif

            services.AddSingleton<IPrintService, XPSPrintService>();

            return services;
        }

        private IServiceCollection ConfigureLive(IServiceCollection services)
        {
            services.AddTransient<IDateTimeProvider, DefaultDateTimeProvider>(sp => new DefaultDateTimeProvider(new DateTime(2018, 3, 15)));

            services.AddTransient<IPatientsRepository, Live.PatientsRepository>(sp => new Live.PatientsRepository(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IResourcesRepository, Live.ResourcesRepository>(sp => new Live.ResourcesRepository(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<ITimeItemsRepository, Live.TimeItemsRepository>(sp => new Live.TimeItemsRepository(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IVisitItemsRepository, Live.VisitItemsRepository>(sp => new Live.VisitItemsRepository(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IDispanserizationsRepository, Live.DispanserizationsRepository>(sp => new Live.DispanserizationsRepository(Configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        private IServiceCollection ConfigureDemo(IServiceCollection services)
        {
            DefaultDateTimeProvider dateTimeProvider = new DefaultDateTimeProvider(new DateTime(2018, 3, 15));
            DemoDataContext dataContext = new DemoDataContext(dateTimeProvider);

            services.AddTransient<IDateTimeProvider, DefaultDateTimeProvider>(sp => dateTimeProvider);
            services.AddTransient<IPatientsRepository, Demo.PatientsRepository>(sp => new Demo.PatientsRepository(dateTimeProvider, dataContext));
            services.AddTransient<IResourcesRepository, Demo.ResourcesRepository>(sp => new Demo.ResourcesRepository(dateTimeProvider, dataContext));
            services.AddTransient<ITimeItemsRepository, Demo.TimeItemsRepository>(sp => new Demo.TimeItemsRepository(dateTimeProvider, dataContext));
            services.AddTransient<IVisitItemsRepository, Demo.VisitItemsRepository>(sp => new Demo.VisitItemsRepository(dateTimeProvider, dataContext));
            services.AddTransient<IDispanserizationsRepository, Demo.DispanserizationsRepository>(sp => new Demo.DispanserizationsRepository(dateTimeProvider, dataContext));

            return services;
        }
    }
}
