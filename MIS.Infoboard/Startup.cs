#region Copyright © 2020 Vladimir Deryagin. All rights reserved
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

namespace MIS.Infoboard
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

			services.Configure<ContactsConfig>(
				Configuration.GetSection($"{nameof(ContactsConfig)}")
			);

			services
				.AddMediatR(typeof(Application.AssemblyMarker));

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

			services.AddSingleton<IResourcesRepository, Live.ResourcesRepository>(sp => new Live.ResourcesRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddSingleton<ITimeItemsRepository, Live.TimeItemsRepository>(sp => new Live.TimeItemsRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddSingleton<IVisitItemsRepository, Live.VisitItemsRepository>(sp => new Live.VisitItemsRepository(Configuration.GetConnectionString("DefaultConnection")));

			return services;
		}

		private IServiceCollection ConfigureDebug(IServiceCollection services)
		{
			services.AddSingleton<IDateTimeProvider, DefaultDateTimeProvider>(sp => new DefaultDateTimeProvider(new System.DateTime(2018, 12, 18)));

			services.AddSingleton<IResourcesRepository, Live.ResourcesRepository>(sp => new Live.ResourcesRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddSingleton<ITimeItemsRepository, Live.TimeItemsRepository>(sp => new Live.TimeItemsRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddSingleton<IVisitItemsRepository, Live.VisitItemsRepository>(sp => new Live.VisitItemsRepository(Configuration.GetConnectionString("DefaultConnection")));

			return services;
		}

		private static IServiceCollection ConfigureDemo(IServiceCollection services)
		{
			var dateTimeProvider = new CurrentDateTimeProvider();
			var dataContext = new DemoDataContext(dateTimeProvider);

			services.AddSingleton<IDateTimeProvider, CurrentDateTimeProvider>(sp => dateTimeProvider);

			services.AddSingleton<IResourcesRepository, Demo.ResourcesRepository>(sp => new Demo.ResourcesRepository(dateTimeProvider, dataContext));
			services.AddSingleton<ITimeItemsRepository, Demo.TimeItemsRepository>(sp => new Demo.TimeItemsRepository(dateTimeProvider, dataContext));
			services.AddSingleton<IVisitItemsRepository, Demo.VisitItemsRepository>(sp => new Demo.VisitItemsRepository(dateTimeProvider, dataContext));

			return services;
		}
	}
}
