using MediatR;
using MIS.Be.Application.Models;

namespace MIS.Be.Application.Queries;

public sealed record GetResourceQuery(int Id) : IRequest<Resource>;
