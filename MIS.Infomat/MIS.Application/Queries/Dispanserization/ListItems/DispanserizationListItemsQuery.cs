using MediatR;
using MIS.Application.ViewModels;
using System.Collections.Generic;

namespace MIS.Application.Queries
{
    public class DispanserizationListItemsQuery : IRequest<IEnumerable<DispanserizationViewModel>>
    {
        public DispanserizationListItemsQuery()
        {
            //
        }
    }
}
