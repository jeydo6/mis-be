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
using MIS.Domain.Providers;
using MIS.Domain.Repositories;

namespace MIS.Infoboard
{
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
			services.AddTransient<IDateTimeProvider, CurrentDateTimeProvider>();

			services.AddTransient<IPatientsRepository, Live.PatientsRepository>(sp => new Live.PatientsRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddTransient<IResourcesRepository, Live.ResourcesRepository>(sp => new Live.ResourcesRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddTransient<ITimeItemsRepository, Live.TimeItemsRepository>(sp => new Live.TimeItemsRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddTransient<IVisitItemsRepository, Live.VisitItemsRepository>(sp => new Live.VisitItemsRepository(Configuration.GetConnectionString("DefaultConnection")));
			services.AddTransient<IDispanserizationsRepository, Live.DispanserizationsRepository>(sp => new Live.DispanserizationsRepository(Configuration.GetConnectionString("DefaultConnection")));

			return services;
		}
	}
}
