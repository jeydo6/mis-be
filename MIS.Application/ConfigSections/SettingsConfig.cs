using System;

namespace MIS.Application.Configs
{
	public class SettingsConfig
	{
		public class ServiceInterval
		{
			public DayOfWeek DayOfWeek { get; set; }

			public string BeginTime { get; set; }

			public string EndTime { get; set; }
		}

		public string OrganizationName { get; set; }

		public int? DispanserizationInterval { get; set; }

		public ServiceInterval[] ServiceIntervals { get; set; }
	}
}
