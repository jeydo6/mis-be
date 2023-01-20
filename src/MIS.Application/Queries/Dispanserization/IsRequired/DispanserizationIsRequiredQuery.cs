using MIS.Application.ViewModels;
using MIS.Mediator;

namespace MIS.Application.Queries
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
