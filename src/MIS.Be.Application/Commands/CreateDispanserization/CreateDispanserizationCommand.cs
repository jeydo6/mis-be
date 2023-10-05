using System;
using MediatR;

namespace MIS.Be.Application.Commands;

public sealed record CreateDispanserizationCommand(int PatientId, DateTimeOffset From) : IRequest;
