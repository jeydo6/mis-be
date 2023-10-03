using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Extensions;
using MIS.Be.Domain.Repositories;
using MIS.Be.Domain.Transactions;

namespace MIS.Be.Application.Commands;

internal sealed class CreateDispanserizationHandler : IRequestHandler<CreateDispanserizationCommand>
{
    private static readonly TimeOnly _timeFrom = new TimeOnly(0, 0, 0);
    private static readonly TimeOnly _timeTo = new TimeOnly(23, 59, 59);

    private readonly ITransactionControl _transactionControl;
    private readonly IDispanserizationsRepository _dispanserizationsRepository;
    private readonly IResearchesRepository _researchesRepository;
    private readonly ITimeItemsRepository _timeItemsRepository;
    private readonly IVisitItemsRepository _visitItemsRepository;

    public CreateDispanserizationHandler(
        ITransactionControl transactionControl,
        IDispanserizationsRepository dispanserizationsRepository,
        IResearchesRepository researchesRepository,
        ITimeItemsRepository timeItemsRepository,
        IVisitItemsRepository visitItemsRepository)
    {
        _transactionControl = transactionControl;
        _dispanserizationsRepository = dispanserizationsRepository;
        _researchesRepository = researchesRepository;
        _timeItemsRepository = timeItemsRepository;
        _visitItemsRepository = visitItemsRepository;
    }

    public async Task Handle(CreateDispanserizationCommand request, CancellationToken cancellationToken)
    {
        var dispanserizations = await _dispanserizationsRepository.GetAll(request.PatientId, cancellationToken);
        if (dispanserizations.Any(d => d.From.Year == request.From.Year))
            throw new ApplicationException($"У пациента с идентификатором '{request.PatientId}' уже существует диспансеризация в '{request.From.Year}' году");

        var from = request.From.ToDateTimeOffset(_timeFrom);
        var to = request.From.ToDateTimeOffset(_timeTo);
        var visitItems = await _visitItemsRepository.GetAll(from, to, cancellationToken: cancellationToken);
        var timeItemIds = visitItems.Select(vi => vi.TimeItemId).ToHashSet();

        var researches = await _researchesRepository.GetAll(cancellationToken);
        var resourceIds = researches
            .Where(r => r.IsDispanserization)
            .Select(r => r.ResourceId)
            .ToHashSet();

        var timeItems = await _timeItemsRepository.GetAll(from, to, cancellationToken: cancellationToken);
        var resourceTimeItemIds = timeItems
            .Where(ti => !timeItemIds.Contains(ti.Id) && resourceIds.Contains(ti.ResourceId))
            .GroupBy(ti => ti.ResourceId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(ti => ti.Id).ToArray()
            );

        var notAvailableResourceIds = resourceIds
            .Where(resourceId => !resourceTimeItemIds.ContainsKey(resourceId) || resourceTimeItemIds[resourceId].Length == 0)
            .ToArray();
        if (notAvailableResourceIds.Length > 0)
            throw new ApplicationException($"Нет доступных слотов для записи у ресурсов с идентификаторами: '{string.Join(", ", notAvailableResourceIds)}'");

        await using var transaction = await _transactionControl.BeginTransaction(cancellationToken: cancellationToken);
        try
        {
            await _dispanserizationsRepository.Create(new Dispanserization
            {
                From = request.From,
                To = new DateOnly(request.From.Year, 12, 31),
                PatientId = request.PatientId
            }, cancellationToken);

            foreach (var timeItemId in resourceIds.Select(resourceId => resourceTimeItemIds[resourceId][0]))
            {
                await _visitItemsRepository.Create(new VisitItem
                {
                    PatientId = request.PatientId,
                    TimeItemId = timeItemId
                }, cancellationToken);
            }

            await transaction.Commit(cancellationToken);
        }
        catch
        {
            await transaction.Rollback(cancellationToken);
            throw;
        }
    }
}
