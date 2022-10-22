using MIS.Domain.Entities;

namespace MIS.Domain.Repositories;

public interface ISpecialtiesRepository
{
	int Create(Specialty item);

	Specialty Get(int id);

	Specialty FindByName(string name);
}
