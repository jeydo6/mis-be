using System.Collections.Generic;
using MIS.Be.Domain.Entities;

namespace MIS.Be.Domain.Repositories
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
