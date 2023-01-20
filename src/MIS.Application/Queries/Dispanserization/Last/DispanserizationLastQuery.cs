using MIS.Application.ViewModels;
using MIS.Mediator;

namespace MIS.Application.Queries
{
	public class DispanserizationLastQuery : IRequest<DispanserizationViewModel>
	{
		public DispanserizationLastQuery(PatientViewModel patient)
		{
			Patient = patient;
		}

		public PatientViewModel Patient { get; }
	}
}
