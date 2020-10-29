using MediatR;
using MIS.Domain.Providers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Application.Queries
{
    public class DateHeaderHandler : IRequestHandler<DateHeaderQuery, String>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public DateHeaderHandler(
            IDateTimeProvider dateTimeProvider
        )
        {
            _dateTimeProvider = dateTimeProvider;
        }


        public async Task<String> Handle(DateHeaderQuery request, CancellationToken cancellationToken)
        {
            DateTime beginDate = _dateTimeProvider.Now.Date;
            Int32 beginDayOfWeek = beginDate.DayOfWeek == 0 ? 7 : (Int32)beginDate.DayOfWeek;

            DateTime endDate = beginDate.AddDays(1 - beginDayOfWeek + 35);

            String header = beginDate.Month == endDate.Month ? $"{beginDate:MMMM}" : $"{beginDate:MMMM}/{endDate:MMMM}";

            return await Task.FromResult(header);
        }
    }
}
