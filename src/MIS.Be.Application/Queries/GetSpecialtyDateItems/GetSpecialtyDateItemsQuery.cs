using System;
using MediatR;
using MIS.Be.Application.Models;

namespace MIS.Be.Application.Queries;

public sealed record GetSpecialtyDateItemsQuery(int SpecialtyId, DateTimeOffset From, DateTimeOffset To) : IRequest<DateItem[]>;
