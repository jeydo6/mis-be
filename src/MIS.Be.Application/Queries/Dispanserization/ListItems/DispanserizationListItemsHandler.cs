using System.Linq;
using MIS.Be.Application.ViewModels;
using MIS.Be.Domain.Providers;
using MIS.Be.Domain.Repositories;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Queries
{
	public class DispanserizationListItemsHandler : IRequestHandler<DispanserizationListItemsQuery, DispanserizationViewModel[]>
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

		public DispanserizationViewModel[] Handle(DispanserizationListItemsQuery request)
		{
			var beginDate = _dateTimeProvider.Now.Date;
			var endDate = _dateTimeProvider.Now.Date.AddDays(28);
			var beginDayOfWeek = beginDate.DayOfWeek == 0 ? 7 : (int)beginDate.DayOfWeek;

			var result = Enumerable
				.Range(1 - beginDayOfWeek, 35)
				.Select(i => new DispanserizationViewModel
				{
					BeginDate = beginDate.AddDays(i)
				})
				.ToArray();

			var resources = _resources.GetDispanserizations();
			var totals = _timeItems.GetDispanserizationTotals(beginDate, endDate);

			var dispanserizationItems = totals
				.GroupBy(t => t.Date)
				.Select(g => new DispanserizationViewModel
				{
					BeginDate = g.Key,
					IsEnabled = g.Count(t => (t.TimesCount - t.VisitsCount) > 0) == resources.Count
				})
				.ToArray();

			if (dispanserizationItems != null && dispanserizationItems.Any())
			{
				var joined = result
					.Join(dispanserizationItems, di => di.BeginDate, d => d.BeginDate, (di, d) =>
					{
						di.IsEnabled = d.IsEnabled;

						return di;
					})
					.ToArray();
			}

			return result;
		}
	}
}
