using Microsoft.Extensions.Options;
using MIS.Application.Configs;
using MIS.Mediator;

namespace MIS.Application.Queries
{
	public class OrganizationNameHandler : IRequestHandler<OrganizationNameQuery, string>
	{
		private readonly SettingsConfig _settingsConfig;

		public OrganizationNameHandler(
			IOptionsSnapshot<SettingsConfig> settingsConfigOptions
		)
		{
			_settingsConfig = settingsConfigOptions.Value;
		}

		public string Handle(OrganizationNameQuery request)
		{
			if(_settingsConfig != null)
			{
				return _settingsConfig.OrganizationName;
			}

			return string.Empty;
		}
	}
}
