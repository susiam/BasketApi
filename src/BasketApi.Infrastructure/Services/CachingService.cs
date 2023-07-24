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

    public T Get<T>(string key)
    {
        return _cache.Get<T>(key);
    }

    public void Set<T>(string key, T data)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions()
               .SetAbsoluteExpiration(TimeSpan.FromDays(1));
        _cache.Set(key, data, cacheEntryOptions);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}
