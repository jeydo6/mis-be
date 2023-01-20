using System.Linq;
using MIS.Be.Application.ViewModels;
using MIS.Be.Domain.Providers;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Queries
{
	public class DateListItemsHandler : IRequestHandler<DateListItemsQuery, DateItemViewModel[]>
	{
		private readonly IDateTimeProvider _dateTimeProvider;

		public DateListItemsHandler(
			IDateTimeProvider dateTimeProvider
		)
		{
			_dateTimeProvider = dateTimeProvider;
		}

		public DateItemViewModel[] Handle(DateListItemsQuery request)
		{
			var beginDate = _dateTimeProvider.Now.Date;
			var beginDayOfWeek = beginDate.DayOfWeek == 0 ? 7 : (int)beginDate.DayOfWeek;

			var result = Enumerable
				.Range(1 - beginDayOfWeek, 35)
				.Select(i => new DateItemViewModel
				{
					Date = beginDate.AddDays(i),
					ResourceID = request.Resource.ResourceID
				})
				.ToArray();

			if (request.Resource.Dates != null && request.Resource.Dates.Any())
			{
				_ = result
					.Join(request.Resource.Dates, di => di.Date, d => d.Date, (di, d) =>
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
