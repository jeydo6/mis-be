using MIS.Application.ViewModels;
using MIS.Mediator;

namespace MIS.Application.Queries
{
	public class DepartmentListItemsQuery : IRequest<DepartmentViewModel[]>
	{
		public DepartmentListItemsQuery()
		{
			//
		}
	}
}
