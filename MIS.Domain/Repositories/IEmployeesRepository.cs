using MIS.Domain.Entities;

namespace MIS.Domain.Repositories;

public interface IEmployeesRepository
{
	int Create(Employee item);
}
