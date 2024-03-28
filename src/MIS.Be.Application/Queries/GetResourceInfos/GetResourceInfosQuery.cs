using System;
using MediatR;
using MIS.Be.Application.Models;

namespace MIS.Be.Application.Queries;

public sealed record GetResourceInfosQuery(int[] ResourceIds, DateTimeOffset From, DateTimeOffset To) : IRequest<ResourceInfo[]>;
