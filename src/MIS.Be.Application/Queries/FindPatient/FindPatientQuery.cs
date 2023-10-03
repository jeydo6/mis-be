using MediatR;
using MIS.Be.Application.Models;

namespace MIS.Be.Application.Queries;

public sealed record FindPatientQuery(string Code, int BirthYear) : IRequest<Patient?>;
