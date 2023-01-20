using MIS.Be.Application.ViewModels;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Queries
{
	public class DispanserizationIsRequiredQuery : IRequest<bool>
	{
		public DispanserizationIsRequiredQuery(PatientViewModel patient)
		{
			Patient = patient;
		}

		public PatientViewModel Patient { get; }
	}
}
