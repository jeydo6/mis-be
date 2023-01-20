using System.Collections.Generic;
using System.Linq;
using MIS.Be.Demo.DataContexts;
using MIS.Be.Domain.Entities;
using MIS.Be.Domain.Providers;
using MIS.Be.Domain.Repositories;

namespace MIS.Be.Demo.Repositories
{
	public class ResourcesRepository : IResourcesRepository
	{
		private readonly DemoDataContext _dataContext;

		public ResourcesRepository(
			IDateTimeProvider _,
			DemoDataContext dataContext
		)
		{
			_dataContext = dataContext;
		}

		public int Create(Resource item)
		{
			throw new System.NotImplementedException();
		}

		public Resource Get(int id)
		{
			throw new System.NotImplementedException();
		}

		public List<Resource> ToList()
		{
			var result = _dataContext.Resources
				.Where(r => r.Employee.Specialty.ID > 0)
				.ToList();

			return result;
		}

		public int CreateDispanserization(Resource item)
		{
			throw new System.NotImplementedException();
		}

		public List<Resource> GetDispanserizations()
		{
			var result = _dataContext.Resources
				.Where(r => r.Employee.Specialty.ID == 0)
				.ToList();

			return result;
		}
	}
}
