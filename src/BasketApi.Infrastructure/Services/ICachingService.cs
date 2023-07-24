namespace BasketApi.Infrastructure.Services;

public interface ICachingService
{
    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> retrieveData);
    T Get<T>(string key);
    void Set<T>(string key, T data);
    void Remove(string key);
}