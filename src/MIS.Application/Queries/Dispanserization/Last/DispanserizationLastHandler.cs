using System.Linq;
using MIS.Application.ViewModels;
using MIS.Domain.Providers;
using MIS.Mediator;

namespace MIS.Application.Queries
{
	public class DispanserizationLastHandler : IRequestHandler<DispanserizationLastQuery, DispanserizationViewModel>
	{
		private readonly IDateTimeProvider _dateTimeProvider;

		public DispanserizationLastHandler(
			IDateTimeProvider dateTimeProvider
		)
		{
			_dateTimeProvider = dateTimeProvider;
		}

		public DispanserizationViewModel Handle(DispanserizationLastQuery request)
		{
			var result = request.Patient.Dispanserizations
				.OrderBy(d => d.BeginDate)
				.LastOrDefault(d => !d.IsClosed && d.BeginDate.Year == _dateTimeProvider.Now.Year);

			return result;
		}
	}
}
