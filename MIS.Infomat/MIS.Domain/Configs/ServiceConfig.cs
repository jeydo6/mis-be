using System;

namespace MIS.Domain.Configs
{
    public class ServiceConfig
    {
        public ServiceInterval[] ServiceIntervals { get; set; }
    }

    public class ServiceInterval
    {
        public DayOfWeek DayOfWeek { get; set; }

        public String BeginTime { get; set; }

        public String EndTime { get; set; }
    }
}
