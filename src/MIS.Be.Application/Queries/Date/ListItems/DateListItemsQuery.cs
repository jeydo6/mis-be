using MIS.Be.Application.ViewModels;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Queries
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
