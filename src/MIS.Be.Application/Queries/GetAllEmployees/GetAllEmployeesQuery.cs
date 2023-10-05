using MediatR;
using MIS.Be.Application.Models;

namespace MIS.Be.Application.Queries;

public record GetAllEmployeesQuery : IRequest<Employee[]>;
