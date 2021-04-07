#region Copyright © 2020-2021 Vladimir Deryagin. All rights reserved
/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.Configs;
using MIS.Demo.DataContexts;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;
using MIS.Domain.Services;
using MIS.Infomat.Services;

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

			services.Configure<SettingsConfig>(
				Configuration.GetSection($"{nameof(SettingsConfig)}")
			);

			services
				.AddMediatR(typeof(Application.AssemblyMarker));

			services
				.AddSingleton<IPrintService, XPSPrintService>();

#if DEMO
			ConfigureDemo(services);
#elif DEBUG
			ConfigureDebug(services);
#else
			ConfigureRelease(services);
#endif

			return services;
		}

		private IServiceCollection ConfigureRelease(IServiceCollection services)
		{
			services.AddSingleton<IDateTimeProvider, CurrentDateTimeProvider>();

			services.AddSingleton<IPatientsRepository, Live.PatientsRepository>(sp => new Live.PatientsRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddSingleton<IResourcesRepository, Live.ResourcesRepository>(sp => new Live.ResourcesRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddSingleton<ITimeItemsRepository, Live.TimeItemsRepository>(sp => new Live.TimeItemsRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddSingleton<IVisitItemsRepository, Live.VisitItemsRepository>(sp => new Live.VisitItemsRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddSingleton<IDispanserizationsRepository, Live.DispanserizationsRepository>(sp => new Live.DispanserizationsRepository(Configuration.GetConnectionString("DefaultConnection")));

			return services;
		}

		private IServiceCollection ConfigureDebug(IServiceCollection services)
		{
			services.AddSingleton<IDateTimeProvider, DefaultDateTimeProvider>(sp => new DefaultDateTimeProvider(new System.DateTime(2018, 12, 18)));

			services.AddSingleton<IPatientsRepository, Live.PatientsRepository>(sp => new Live.PatientsRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddSingleton<IResourcesRepository, Live.ResourcesRepository>(sp => new Live.ResourcesRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddSingleton<ITimeItemsRepository, Live.TimeItemsRepository>(sp => new Live.TimeItemsRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddSingleton<IVisitItemsRepository, Live.VisitItemsRepository>(sp => new Live.VisitItemsRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddSingleton<IDispanserizationsRepository, Live.DispanserizationsRepository>(sp => new Live.DispanserizationsRepository(Configuration.GetConnectionString("DefaultConnection")));

			return services;
		}

		private static IServiceCollection ConfigureDemo(IServiceCollection services)
		{
			var dateTimeProvider = new CurrentDateTimeProvider();
			var dataContext = new DemoDataContext(dateTimeProvider);

			services.AddSingleton<IDateTimeProvider, CurrentDateTimeProvider>(sp => dateTimeProvider);

			services.AddSingleton<IPatientsRepository, Demo.PatientsRepository>(sp => new Demo.PatientsRepository(dateTimeProvider, dataContext));
			services.AddSingleton<IResourcesRepository, Demo.ResourcesRepository>(sp => new Demo.ResourcesRepository(dateTimeProvider, dataContext));
			services.AddSingleton<ITimeItemsRepository, Demo.TimeItemsRepository>(sp => new Demo.TimeItemsRepository(dateTimeProvider, dataContext));
			services.AddSingleton<IVisitItemsRepository, Demo.VisitItemsRepository>(sp => new Demo.VisitItemsRepository(dateTimeProvider, dataContext));
			services.AddSingleton<IDispanserizationsRepository, Demo.DispanserizationsRepository>(sp => new Demo.DispanserizationsRepository(dateTimeProvider, dataContext));

			return services;
		}
	}
}
