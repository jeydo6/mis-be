using MediatR;
using Microsoft.Extensions.Options;
using MIS.Domain.Configs;
using MIS.Domain.Providers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Queries
{
    public class TimeIsServiceHandler : IRequestHandler<TimeIsServiceQuery, Boolean>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        private readonly ServiceInterval[] _serviceIntervals;

        public TimeIsServiceHandler(
            IDateTimeProvider dateTimeProvider,
            IOptionsSnapshot<ServiceConfig> serviceConfigOptions
        )
        {
            _dateTimeProvider = dateTimeProvider;
            _serviceIntervals = serviceConfigOptions.Value.ServiceIntervals;
        }

        public async Task<Boolean> Handle(TimeIsServiceQuery request, CancellationToken cancellationToken)
        {
            if (_serviceIntervals != null)
            {
                DayOfWeek dayOfWeek = _dateTimeProvider.Now.Date.DayOfWeek;
                TimeSpan timeOfDay = _dateTimeProvider.Now.TimeOfDay;

                Boolean isService = _serviceIntervals.Any(si =>
                {
                    TimeSpan beginService = DateTime.Parse(si.BeginTime).TimeOfDay;
                    TimeSpan endService = DateTime.Parse(si.EndTime).TimeOfDay;

                    return timeOfDay >= beginService && timeOfDay < endService;
                });

                return await Task.FromResult(isService);
            }

            return false;
        }
    }
}
