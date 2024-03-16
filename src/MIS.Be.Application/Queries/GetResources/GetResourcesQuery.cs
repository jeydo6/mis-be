using MediatR;
using MIS.Be.Application.Models;

namespace MIS.Be.Application.Queries;

public sealed record GetResourcesQuery(int[] Ids) : IRequest<Resource[]>;
