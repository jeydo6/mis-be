using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories;

public interface ISpecialtiesRepository
{
	int Create(Specialty item);

	Specialty Get(int id);
}
