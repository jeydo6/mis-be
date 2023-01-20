using MIS.Application.ViewModels;
using MIS.Mediator;

namespace MIS.Application.Queries
{
	public class DateListItemsQuery : IRequest<DateItemViewModel[]>
	{
		public DateListItemsQuery(ResourceViewModel resource)
		{
			Resource = resource;
		}

		public ResourceViewModel Resource { get; }
	}
}
