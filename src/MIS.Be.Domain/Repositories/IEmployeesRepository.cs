using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories;

public interface IEmployeesRepository
{
	int Create(Employee item);

	Employee Get(int id);
}
