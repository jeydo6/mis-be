using System.Linq;
using MIS.Be.Domain.Providers;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Queries
{
	public class DispanserizationIsRequiredHandler : IRequestHandler<DispanserizationIsRequiredQuery, bool>
	{
		private readonly IDateTimeProvider _dateTimeProvider;

		public DispanserizationIsRequiredHandler(
			IDateTimeProvider dateTimeProvider
		)
		{
			_dateTimeProvider = dateTimeProvider;
		}

		public bool Handle(DispanserizationIsRequiredQuery request)
		{
			var result = !request.Patient.Dispanserizations
				.Any(d => d.BeginDate.Year == _dateTimeProvider.Now.Year);

			return result;
		}
	}
}
