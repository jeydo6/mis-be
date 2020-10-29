using MediatR;
using MIS.Application.ViewModels;
using System.Collections.Generic;

namespace MIS.Application.Queries
{
    public class DateListItemsQuery : IRequest<IEnumerable<DateItemViewModel>>
    {
        public DateListItemsQuery(ResourceViewModel resource)
        {
            Resource = resource;
        }

        public ResourceViewModel Resource { get; }
    }
}
