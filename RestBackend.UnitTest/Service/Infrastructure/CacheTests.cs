using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using RestBackend.Infrastructure.Cache;

namespace RestBackend.UnitTest.Service.Infrastructure
{
    public class Tests
    {
        private IMemoryCache memoryCache;

        [SetUp]
        public void Setup()
        {
            memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [Test]
        public void CacheServiceDataTest()
        {
            int toStoreValue = 1991;
            CacheService cacheService = new CacheService(memoryCache);
            cacheService.GetOrCreate<int>("PASS_KEY", () => toStoreValue);

            var fromCacheValue = cacheService.GetOrCreate<int>("PASS_KEY", () => -1);
            
            Assert.IsTrue(toStoreValue == fromCacheValue);
        }
    }
}