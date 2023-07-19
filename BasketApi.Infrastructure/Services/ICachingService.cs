namespace BasketApi.Infrastructure.Services;

public interface ICachingService
{
    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> retrieveData);
}