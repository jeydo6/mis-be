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

                    return dayOfWeek == si.DayOfWeek
                        && timeOfDay >= beginService
                        && timeOfDay < endService;
                });

                return await Task.FromResult(isService);
            }

            return false;
        }
    }
}
