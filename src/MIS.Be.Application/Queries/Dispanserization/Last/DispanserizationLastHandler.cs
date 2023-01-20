using System.Linq;
using MIS.Be.Application.ViewModels;
using MIS.Be.Domain.Providers;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Queries
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
