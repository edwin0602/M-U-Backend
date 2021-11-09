using Microsoft.Extensions.Caching.Memory;
using RestBackend.Core.Services.Infrastructure;
using System;

namespace RestBackend.Infrastructure.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public TItem GetOrCreate<TItem>(object key, Func<TItem> createItem)
        {
            if (!_cache.TryGetValue(key, out TItem cacheEntry))
            {
                cacheEntry = createItem();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                                            .SetPriority(CacheItemPriority.High)
                                            .SetAbsoluteExpiration(DateTime.Now.AddHours(12));

                _cache.Set(key, cacheEntry, cacheEntryOptions);
            }

            return cacheEntry;
        }
    }
}
