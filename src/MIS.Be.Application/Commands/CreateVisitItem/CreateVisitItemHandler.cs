using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Filters;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Application.Commands;

internal sealed class CreateVisitItemHandler : IRequestHandler<CreateVisitItemCommand>
{
    private readonly IResourcesRepository _resourcesRepository;
    private readonly ITimeItemsRepository _timeItemsRepository;
    private readonly IVisitItemsRepository _visitItemsRepository;

    public CreateVisitItemHandler(
        IResourcesRepository resourcesRepository,
        ITimeItemsRepository timeItemsRepository,
        IVisitItemsRepository visitItemsRepository)
    {
        _resourcesRepository = resourcesRepository;
        _timeItemsRepository = timeItemsRepository;
        _visitItemsRepository = visitItemsRepository;
    }

    public async Task Handle(CreateVisitItemCommand request, CancellationToken cancellationToken)
    {
        var visitItems = await _visitItemsRepository.GetAll(DateTimeOffset.UtcNow, DateTimeOffset.MaxValue,
            filter: new GetAllVisitItemsFilter(PatientId: request.PatientId),
            cancellationToken: cancellationToken);

        var timeItemIds = visitItems.Select(vi => vi.TimeItemId).ToArray();
        var timeItems = await _timeItemsRepository.Get(timeItemIds, cancellationToken);
        var resources = await _resourcesRepository.GetAll(cancellationToken);

        var resourceIds = timeItems.Select(ti => ti.ResourceId).ToHashSet();
        var specialtyIds = resources
            .Where(r => !r.IsDispanserization && resourceIds.Contains(r.Id))
            .Select(r => r.SpecialtyId)
            .ToHashSet();

        var timeItem = await _timeItemsRepository.Get(request.TimeItemId, cancellationToken);
        var resource = resources.First(r => r.Id == timeItem.ResourceId);
        if (specialtyIds.Contains(resource.SpecialtyId))
            throw new ApplicationException($"У пациента с идентификатором '{request.PatientId}' уже существует запись к специальности с идентификатором '{resource.SpecialtyId}'");

        await _visitItemsRepository.Create(new VisitItem
        {
            PatientId = request.PatientId,
            TimeItemId = request.TimeItemId
        }, cancellationToken);
    }
}
