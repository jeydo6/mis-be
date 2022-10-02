#region Copyright © 2018-2022 Vladimir Deryagin. All rights reserved
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

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.Configs;
using MIS.Demo.DataContexts;
using MIS.Domain.Options;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;
using MIS.Mediator;

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
				.AddMediator(typeof(Application.AssemblyMarker));

#if DEMO
			services
				.ConfigureDemo();
#elif DEBUG
			services
				.ConfigureDebug(options =>
				{
					options.ConnectionString = Configuration
						.GetConnectionString("DefaultConnection");
				});
#elif RELEASE
			services
				.ConfigureRelease(options =>
				{
					options.ConnectionString = Configuration
						.GetConnectionString("DefaultConnection");
				});
#else
			throw new Exception("Unknown project configuration!");
#endif

			return services;
		}
	}

	internal static class StartupExtension
	{
		public static IServiceCollection ConfigureRelease(this IServiceCollection services, Action<LiveServicesOptions> configureOptions)
		{
			var liveOptions = new LiveServicesOptions();
			configureOptions?.Invoke(liveOptions);

			services
				.AddSingleton<IDateTimeProvider, CurrentDateTimeProvider>();

			services
				.AddSingleton<IResourcesRepository, Live.ResourcesRepository>(sp => new Live.ResourcesRepository(liveOptions.ConnectionString))
				.AddSingleton<ITimeItemsRepository, Live.TimeItemsRepository>(sp => new Live.TimeItemsRepository(liveOptions.ConnectionString))
				.AddSingleton<IVisitItemsRepository, Live.VisitItemsRepository>(sp => new Live.VisitItemsRepository(liveOptions.ConnectionString));

			return services;
		}

		public static IServiceCollection ConfigureDebug(this IServiceCollection services, Action<LiveServicesOptions> configureOptions)
		{
			var liveOptions = new LiveServicesOptions();
			configureOptions?.Invoke(liveOptions);

			services
				.AddSingleton<IDateTimeProvider, DefaultDateTimeProvider>(sp => new DefaultDateTimeProvider(new System.DateTime(2022, 5, 10)));

			services
				.AddSingleton<IResourcesRepository, Live.ResourcesRepository>(sp => new Live.ResourcesRepository(liveOptions.ConnectionString))
				.AddSingleton<ITimeItemsRepository, Live.TimeItemsRepository>(sp => new Live.TimeItemsRepository(liveOptions.ConnectionString))
				.AddSingleton<IVisitItemsRepository, Live.VisitItemsRepository>(sp => new Live.VisitItemsRepository(liveOptions.ConnectionString));

			return services;
		}

		public static IServiceCollection ConfigureDemo(this IServiceCollection services)
		{
			services
				.AddSingleton<IDateTimeProvider, CurrentDateTimeProvider>();

			services
				.AddSingleton<DemoDataContext>();

			services
				.AddSingleton<IResourcesRepository, Demo.ResourcesRepository>()
				.AddSingleton<ITimeItemsRepository, Demo.TimeItemsRepository>()
				.AddSingleton<IVisitItemsRepository, Demo.VisitItemsRepository>();

			return services;
		}
	}
}
