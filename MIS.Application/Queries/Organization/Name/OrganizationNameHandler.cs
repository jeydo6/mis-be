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
