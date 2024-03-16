using MediatR;
using MIS.Be.Application.Models;

namespace MIS.Be.Application.Queries;

public sealed record GetSpecialtyResourcesQuery(int SpecialtyId) : IRequest<Resource[]>;
