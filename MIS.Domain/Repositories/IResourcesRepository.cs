using System.Collections.Generic;
using MIS.Domain.Entities;

namespace MIS.Domain.Repositories
{
	public interface IResourcesRepository
	{
		int Create(Resource item);

		Resource Get(int id);

		List<Resource> ToList();

		int CreateDispanserization(Resource item);

		List<Resource> GetDispanserizations();
	}
}
