using System;
using MediatR;
using MIS.Be.Application.Models;

namespace MIS.Be.Application.Queries;

public sealed record GetResourceDateItemsQuery(int ResourceId, DateTimeOffset From, DateTimeOffset To) : IRequest<DateItem[]>;
