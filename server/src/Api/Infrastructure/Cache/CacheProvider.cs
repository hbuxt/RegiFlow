using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Api.Infrastructure.Cache
{
    internal sealed class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CacheProvider> _logger;

        public CacheProvider(
            IMemoryCache memoryCache,
            ILogger<CacheProvider> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }
        
        public async Task<TValue?> ReadThroughAsync<TValue>(string cacheKey, ICacheOptions cacheOptions, Func<Task<TValue?>> source) where TValue : class
        {
            try
            {
                if (cacheOptions.IsEnabled)
                {
                    var isInCache = _memoryCache.TryGetValue<TValue>(cacheKey, out var cachedData);

                    if (isInCache)
                    {
                        return cachedData;
                    }
                }

                var data = await source();

                if (cacheOptions.IsEnabled)
                {
                    _memoryCache.Set(cacheKey, data, new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(cacheOptions.LengthInMinutes)
                    });
                }

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cache: {CacheKey} retrieval failed", cacheKey);
                throw;
            }
        }
        
        public void Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }
        
        public void Remove(string[] cacheKeys)
        {
            foreach (var cacheKey in cacheKeys)
            {
                _memoryCache.Remove(cacheKey);
            }
        }
    }
}