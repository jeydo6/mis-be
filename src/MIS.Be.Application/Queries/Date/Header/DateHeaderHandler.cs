using MIS.Be.Domain.Providers;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Queries
{
	public class DateHeaderHandler : IRequestHandler<DateHeaderQuery, string>
	{
		private readonly IDateTimeProvider _dateTimeProvider;

		public DateHeaderHandler(
			IDateTimeProvider dateTimeProvider
		)
		{
			_dateTimeProvider = dateTimeProvider;
		}


		public string Handle(DateHeaderQuery request)
		{
			var beginDate = _dateTimeProvider.Now.Date;
			var beginDayOfWeek = beginDate.DayOfWeek == 0 ? 7 : (int)beginDate.DayOfWeek;

			var endDate = beginDate.AddDays(1 - beginDayOfWeek + 35);

			var result = beginDate.Month == endDate.Month ? $"{beginDate:MMMM}" : $"{beginDate:MMMM}/{endDate:MMMM}";

			return result;
		}
	}
}
