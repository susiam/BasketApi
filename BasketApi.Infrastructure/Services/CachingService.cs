using BasketApi.Infrastructure.Clients;
using Microsoft.Extensions.Caching.Memory;

namespace BasketApi.Infrastructure.Services;

public sealed class CachingService : ICachingService
{
    private readonly IMemoryCache _cache;

    public CachingService(IMemoryCache cache)
    {
        ArgumentNullException.ThrowIfNull(cache);
        _cache = cache;
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> retrieveData)
    {
        if (!_cache.TryGetValue(key, out T data))
        {
            data = await retrieveData();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            _cache.Set(key, data, cacheEntryOptions);
        }

        return data;
    }
}
