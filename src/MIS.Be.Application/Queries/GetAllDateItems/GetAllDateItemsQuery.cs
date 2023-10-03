using System;
using MediatR;
using MIS.Be.Application.Models;

namespace MIS.Be.Application.Queries;

public sealed record GetAllDateItemsQuery(DateTimeOffset From, DateTimeOffset To) : IRequest<DateItem[]>;
