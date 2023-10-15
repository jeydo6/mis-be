using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Application.Extensions;
using MIS.Be.Application.Models;
using MIS.Be.Domain.Filters;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Queries;

internal sealed class GetPatientVisitItemsHandler : IRequestHandler<GetPatientVisitItemsQuery, VisitItem[]>
{
    private readonly IVisitItemsRepository _visitItemsRepository;

    public GetPatientVisitItemsHandler(IVisitItemsRepository visitItemsRepository)
    {
        _visitItemsRepository = visitItemsRepository;
    }

    public async Task<VisitItem[]> Handle(GetPatientVisitItemsQuery request, CancellationToken cancellationToken)
    {
        var visitItems = await _visitItemsRepository.GetAll(request.From, request.To,
            filter: new GetAllVisitItemsFilter(PatientId: request.PatientId),
            cancellationToken: cancellationToken);

        return visitItems
            .Select(MappingExtension.Map)
            .OrderBy(vi => vi.Id)
            .ToArray();
    }
}
