using MIS.Domain.Repositories;
using MIS.Persistence.Repositories;
using MIS.Tests.Fixtures.Live;
using Xunit;

namespace MIS.Tests.Repositories.Live
{
    public class ResourcesRepositoryTests : IClassFixture<DataFixture>
    {
        private readonly IResourcesRepository _resources;

        public ResourcesRepositoryTests(DataFixture dataFixture)
        {
            _resources = new ResourcesRepository(dataFixture.Transaction);
        }

        [Fact]
        public void ToList_Ok()
        {
            var actualResult = _resources.ToList();

            Assert.NotEmpty(actualResult);
        }

        [Fact]
        public void GetDispanserizations_Ok()
        {
            var actualResult = _resources.GetDispanserizations();

            Assert.NotEmpty(actualResult);
        }
    }
}
