
namespace FlightStorageService.Caching;

public interface ICacheHelper
{
    void AddToCache<T>(string cacheKey, T data);
    T GetFromCache<T>(string cacheKey);
    Task<T> GetOrCreate<T>(string cacheKey, Func<Task<T>> getData);
    void Remove();
}