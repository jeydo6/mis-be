using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories;

public interface IRoomsRepository
{
	int Create(Room item);

	Room Get(int id);
}
