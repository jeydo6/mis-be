using MediatR;
using MIS.Application.ViewModels;
using MIS.Domain.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Queries
{
    public class DateListItemsHandler : IRequestHandler<DateListItemsQuery, IEnumerable<DateItemViewModel>>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public DateListItemsHandler(
            IDateTimeProvider dateTimeProvider
        )
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<IEnumerable<DateItemViewModel>> Handle(DateListItemsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<DateItemViewModel> viewModels = null;

            if (request.Resource.Dates != null && request.Resource.Dates.Count() > 0)
            {
                DateTime beginDate = _dateTimeProvider.Now.Date;
                Int32 beginDayOfWeek = beginDate.DayOfWeek == 0 ? 7 : (Int32)beginDate.DayOfWeek;

                viewModels = Enumerable
                    .Range(1 - beginDayOfWeek, 35)
                    .Select(i => new DateItemViewModel
                    {
                        Date = beginDate.AddDays(i),
                        ResourceID = request.Resource.ResourceID
                    })
                    .ToList();

                viewModels
                    .Join(request.Resource.Dates, di => di.Date, d => d.Date, (di, d) =>
                    {
                        di.IsEnabled = d.IsEnabled;

                        return di;
                    })
                    .ToList();
            }

            return await Task.FromResult(viewModels);
        }
    }
}
