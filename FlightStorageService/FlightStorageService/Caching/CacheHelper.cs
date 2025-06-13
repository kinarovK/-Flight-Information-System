using Microsoft.Extensions.Caching.Memory;

namespace FlightStorageService.Caching;

public class CacheHelper : ICacheHelper
{
    private readonly IMemoryCache _cache;

    public CacheHelper(IMemoryCache cache)
    {
        _cache = cache;
    }
    public async Task<T> GetOrCreate<T>(string cacheKey, Func<Task<T>> getData)
    {
        T cachedValue = GetFromCache<T>(cacheKey);
        if (cachedValue != null)
        {
            return cachedValue;
        }
        T data = await getData();

        AddToCache(cacheKey, data);
        return data;

    }

    public T GetFromCache<T>(string cacheKey)
    {
        if (_cache.TryGetValue(cacheKey, out T cachedValue))
        {
            return cachedValue;
        }
        return default;
    }

    public void AddToCache<T>(string cacheKey, T data)
    {
        _cache.Set(cacheKey, data, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(5)
        });
    }
    public void Remove()
    {
        if (_cache is MemoryCache concreteMemoryCache)
        {
            concreteMemoryCache.Clear();
        }
    }
}
