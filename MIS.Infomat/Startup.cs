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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.Configs;
using MIS.Domain.Services;
using MIS.Infomat.Services;
using MIS.Mediator;
using MIS.Persistence.Extensions;

namespace MIS.Infomat
{
	internal sealed class Startup
	{
		public Startup(IConfiguration configuration) =>
			Configuration = configuration;

		public IConfiguration Configuration { get; }

		public IServiceCollection ConfigureServices(IServiceCollection services)
		{
			services.Configure<SettingsConfig>(
				Configuration.GetSection($"{nameof(SettingsConfig)}")
			);

			services
				.AddMediator(typeof(Application.AssemblyMarker));

			services
				.AddSingleton<IPrintService, XPSPrintService>();

#if DEMO
			services
				.ConfigureDemo();
#elif DEBUG
			services
				.ConfigureDebug();
#elif RELEASE
			services
				.ConfigureRelease();
#else
			throw new Exception("Unknown project configuration!");
#endif

			return services;
		}
	}
}
