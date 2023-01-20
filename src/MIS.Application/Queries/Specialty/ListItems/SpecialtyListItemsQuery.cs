using MIS.Application.ViewModels;
using MIS.Mediator;

namespace MIS.Application.Queries
{
	public class SpecialtyListItemsQuery : IRequest<SpecialtyViewModel[]>
	{
		public SpecialtyListItemsQuery(PatientViewModel patient)
		{
			Patient = patient;
		}

		public PatientViewModel Patient { get; }
	}
}
