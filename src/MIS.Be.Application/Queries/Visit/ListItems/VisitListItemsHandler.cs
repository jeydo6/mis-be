using System.Linq;
using MIS.Be.Application.ViewModels;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Queries
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
