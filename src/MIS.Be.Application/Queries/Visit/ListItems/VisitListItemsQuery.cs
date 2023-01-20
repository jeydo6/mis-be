using MIS.Be.Application.ViewModels;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Queries
{
	public class VisitListItemsQuery : IRequest<VisitItemViewModel[]>
	{
		public VisitListItemsQuery(PatientViewModel patient)
		{
			Patient = patient;
		}

		public PatientViewModel Patient { get; }
	}
}
