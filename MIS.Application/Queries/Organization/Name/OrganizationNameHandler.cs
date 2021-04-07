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
using Microsoft.Extensions.Options;
using MIS.Application.Configs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Queries
{
	public class OrganizationNameHandler : IRequestHandler<OrganizationNameQuery, String>
	{
		private readonly SettingsConfig _settingsConfig;

		public OrganizationNameHandler(
			IOptionsSnapshot<SettingsConfig> settingsConfigOptions
		)
		{
			_settingsConfig = settingsConfigOptions.Value;
		}

		public async Task<String> Handle(OrganizationNameQuery request, CancellationToken cancellationToken)
		{
			if (_settingsConfig != null)
			{
				return await Task.FromResult(_settingsConfig.OrganizationName);
			}

			return await Task.FromResult<String>(null);
		}
	}
}
