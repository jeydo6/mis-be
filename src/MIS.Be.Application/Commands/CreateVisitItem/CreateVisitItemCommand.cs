using MediatR;

namespace MIS.Be.Application.Commands;

public sealed record CreateVisitItemCommand(int PatientId, int TimeItemId) : IRequest;
