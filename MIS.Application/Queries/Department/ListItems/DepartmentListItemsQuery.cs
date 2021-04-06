using MediatR;
using MIS.Application.ViewModels;

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
