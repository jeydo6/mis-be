using MIS.Demo.DataContexts;
using MIS.Domain.Entities;
using MIS.Domain.Providers;
using MIS.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MIS.Demo.Repositories
{
    public class ResourcesRepository : IResourcesRepository
    {
        private readonly DemoDataContext _dataContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ResourcesRepository(
            IDateTimeProvider dateTimeProvider,
            DemoDataContext dataContext
        )
        {
            _dateTimeProvider = dateTimeProvider;
            _dataContext = dataContext;
        }

        public IEnumerable<Resource> ToList()
        {
            return _dataContext.Resources
                .Where(r => r.Doctor.Specialty.ID > 0)
                .ToList();
        }

        public IEnumerable<Resource> GetDispanserizations()
        {
            return _dataContext.Resources
                .Where(r => r.Doctor.Specialty.ID == 0)
                .ToList();
        }
    }
}