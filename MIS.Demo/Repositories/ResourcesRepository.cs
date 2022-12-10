using System.Collections.Generic;
using System.Linq;
using MIS.Demo.DataContexts;
using MIS.Domain.Entities;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;

namespace MIS.Demo.Repositories
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
