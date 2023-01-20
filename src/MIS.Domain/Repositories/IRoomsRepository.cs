using MIS.Domain.Entities;

namespace MIS.Domain.Repositories;

public interface IRoomsRepository
{
	int Create(Room item);

	Room Get(int id);
}
