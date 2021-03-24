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

			services
				.AddMediatR(typeof(Application.AssemblyMarker));

#if DEMO
			ConfigureDemo(services);
#else
			ConfigureLive(services);
#endif

			return services;
		}

		private IServiceCollection ConfigureLive(IServiceCollection services)
		{
			//services.AddTransient<IDateTimeProvider, CurrentDateTimeProvider>();

			services.AddTransient<IDateTimeProvider, DefaultDateTimeProvider>(sp => new DefaultDateTimeProvider(new System.DateTime(2018, 12, 15)));

			services.AddTransient<IResourcesRepository, Live.ResourcesRepository>(sp => new Live.ResourcesRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddTransient<ITimeItemsRepository, Live.TimeItemsRepository>(sp => new Live.TimeItemsRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddTransient<IVisitItemsRepository, Live.VisitItemsRepository>(sp => new Live.VisitItemsRepository(Configuration.GetConnectionString("DefaultConnection")));

			return services;
		}

		private static IServiceCollection ConfigureDemo(IServiceCollection services)
		{
			var dateTimeProvider = new CurrentDateTimeProvider();
			var dataContext = new DemoDataContext(dateTimeProvider);

			services.AddTransient<IDateTimeProvider, CurrentDateTimeProvider>(sp => dateTimeProvider);

			services.AddTransient<IResourcesRepository, Demo.ResourcesRepository>(sp => new Demo.ResourcesRepository(dateTimeProvider, dataContext));
			services.AddTransient<ITimeItemsRepository, Demo.TimeItemsRepository>(sp => new Demo.TimeItemsRepository(dateTimeProvider, dataContext));
			services.AddTransient<IVisitItemsRepository, Demo.VisitItemsRepository>(sp => new Demo.VisitItemsRepository(dateTimeProvider, dataContext));

			return services;
		}
	}
}
