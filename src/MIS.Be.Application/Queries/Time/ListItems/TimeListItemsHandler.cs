using System.Linq;
using MIS.Be.Application.ViewModels;
using MIS.Be.Domain.Providers;
using MIS.Be.Domain.Repositories;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Queries
{
	public class TimeListItemsHandler : IRequestHandler<TimeListItemsQuery, TimeItemViewModel[]>
	{
		private readonly ITimeItemsRepository _timeItems;
		private readonly IDateTimeProvider _dateTimeProvider;

		public TimeListItemsHandler(
			IDateTimeProvider dateTimeProvider,
			ITimeItemsRepository timeItems
		)
		{
			_dateTimeProvider = dateTimeProvider;
			_timeItems = timeItems;
		}

		public TimeItemViewModel[] Handle(TimeListItemsQuery request)
		{
			var timeItems = _timeItems
				.ToList(request.Date, request.Date, request.ResourceID);

			var result = timeItems
				.Where(t => t.BeginDateTime > _dateTimeProvider.Now)
				.Select(t => new TimeItemViewModel
				{
					TimeItemID = t.ID,
					DateTime = t.BeginDateTime,
					IsEnabled = t.VisitItem == null
				})
				.OrderBy(ti => ti.DateTime)
				.Take(28)
				.ToArray();

			return result;
		}
	}
}
