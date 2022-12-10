using System.Linq;
using MIS.Application.ViewModels;
using MIS.Mediator;

namespace MIS.Application.Queries
{
	public class VisitListItemsHandler : IRequestHandler<VisitListItemsQuery, VisitItemViewModel[]>
	{
		public VisitItemViewModel[] Handle(VisitListItemsQuery request)
		{
			var result = request.Patient.VisitItems
				.OrderBy(v => v.BeginDateTime)
				.ToArray();

			return result;
		}
	}
}
