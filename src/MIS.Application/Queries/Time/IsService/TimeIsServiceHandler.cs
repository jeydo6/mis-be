using System;
using System.Linq;
using Microsoft.Extensions.Options;
using MIS.Application.Configs;
using MIS.Domain.Providers;
using MIS.Mediator;

namespace MIS.Application.Queries
{
	public class TimeIsServiceHandler : IRequestHandler<TimeIsServiceQuery, bool>
	{
		private readonly IDateTimeProvider _dateTimeProvider;

		private readonly SettingsConfig _settingsConfig;

		public TimeIsServiceHandler(
			IDateTimeProvider dateTimeProvider,
			IOptionsSnapshot<SettingsConfig> settingsConfigOptions
		)
		{
			_dateTimeProvider = dateTimeProvider;
			_settingsConfig = settingsConfigOptions.Value;
		}

		public bool Handle(TimeIsServiceQuery request)
		{
			if (_settingsConfig != null && _settingsConfig.ServiceIntervals != null)
			{
				var dayOfWeek = _dateTimeProvider.Now.Date.DayOfWeek;
				var timeOfDay = _dateTimeProvider.Now.TimeOfDay;

				var result = _settingsConfig.ServiceIntervals.Any(si =>
				{
					var beginService = DateTime.Parse(si.BeginTime).TimeOfDay;
					var endService = DateTime.Parse(si.EndTime).TimeOfDay;

					return dayOfWeek == si.DayOfWeek
						&& timeOfDay >= beginService
						&& timeOfDay < endService;
				});

				return result;
			}

			return false;
		}
	}
}
