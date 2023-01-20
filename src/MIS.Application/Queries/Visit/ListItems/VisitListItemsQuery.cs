using MIS.Application.ViewModels;
using MIS.Mediator;

namespace MIS.Application.Queries
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
