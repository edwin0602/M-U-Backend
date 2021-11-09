using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using RestBackend.Core.Models.Business;
using RestBackend.Data;
using RestBackend.Infrastructure.Cache;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestBackend.UnitTest.Service
{
    public class TaxServiceTest
    {
        private readonly List<Tax> seedData = new();
        private UnitOfWork unitOfWork;
        private CacheService cacheService;

        [SetUp]
        public void Setup()
        {
            seedData.Add(new Tax { Enabled = false, IdTax = 1, Value = 0.17m });
            seedData.Add(new Tax { Enabled = true, IdTax = 2, Value = 0.18m });
            seedData.Add(new Tax { Enabled = true, IdTax = 3, Value = 0.19m });

            var MockContext = new Mock<RestBackendDbContext>();
            MockContext.Setup(x => x.Set<Tax>()).ReturnsDbSet(seedData);

            unitOfWork = new UnitOfWork(MockContext.Object);

            IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
            cacheService = new CacheService(memoryCache);
        }

        [Test]
        public async Task TestGetActiveTax()
        {
            var activeTax = seedData
                .OrderByDescending(o => o.IdTax)
                .FirstOrDefault(x => x.Enabled).Value;

            var TaxService = new Services.TaxService(unitOfWork, cacheService);
            var serviceResult = await TaxService.GetCurrentTax();

            Assert.IsTrue(activeTax == serviceResult);
        }

    }
}
