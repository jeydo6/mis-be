using MIS.Domain.Entities;
using System.Collections.Generic;

namespace MIS.Domain.Repositories
{
    public interface IResourcesRepository
    {
        IEnumerable<Resource> ToList();

        IEnumerable<Resource> GetDispanserizations();
    }
}
