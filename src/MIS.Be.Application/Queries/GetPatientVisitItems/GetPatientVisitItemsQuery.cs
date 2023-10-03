using System;
using MediatR;
using MIS.Be.Application.Models;

namespace MIS.Be.Application.Queries;

public record GetPatientVisitItemsQuery(int PatientId, DateTimeOffset From, DateTimeOffset To) : IRequest<VisitItem[]>;
