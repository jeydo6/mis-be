using MIS.Be.Application.ViewModels;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Queries
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
