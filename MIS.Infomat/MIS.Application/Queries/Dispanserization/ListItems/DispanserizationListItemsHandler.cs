using MediatR;
using MIS.Application.ViewModels;
using MIS.Domain.Entities;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Queries
{
    public class DispanserizationListItemsHandler : IRequestHandler<DispanserizationListItemsQuery, IEnumerable<DispanserizationViewModel>>
    {

        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IResourcesRepository _resources;
        private readonly ITimeItemsRepository _timeItems;

        public DispanserizationListItemsHandler(
            IDateTimeProvider dateTimeProvider,
            IResourcesRepository resources,
            ITimeItemsRepository timeItems
        )
        {
            _dateTimeProvider = dateTimeProvider;
            _resources = resources;
            _timeItems = timeItems;
        }

        public async Task<IEnumerable<DispanserizationViewModel>> Handle(DispanserizationListItemsQuery request, CancellationToken cancellationToken)
        {
            DateTime beginDate = _dateTimeProvider.Now.Date;
            DateTime endDate = _dateTimeProvider.Now.Date.AddDays(28);

            Int32 resourcesCount = _resources.GetDispanserizations().Count();
            IEnumerable<TimeItemTotal> totals = _timeItems.GetDispanserizationTotals(beginDate, endDate);

            IEnumerable<DispanserizationViewModel> dispanserizationItems = totals
                .GroupBy(t => t.Date)
                .Select(g => new DispanserizationViewModel
                {
                    BeginDate = g.Key,
                    IsEnabled = g.Count() == resourcesCount
                })
                .ToList();

            IEnumerable<DispanserizationViewModel> viewModels = null;

            if (dispanserizationItems != null && dispanserizationItems.Count() > 0)
            {
                Int32 beginDayOfWeek = beginDate.DayOfWeek == 0 ? 7 : (Int32)beginDate.DayOfWeek;

                viewModels = Enumerable
                    .Range(1 - beginDayOfWeek, 35)
                    .Select(i => new DispanserizationViewModel
                    {
                        BeginDate = beginDate.AddDays(i)
                    })
                    .ToList();

                viewModels
                    .Join(dispanserizationItems, di => di.BeginDate, d => d.BeginDate, (di, d) =>
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
