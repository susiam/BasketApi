using BasketApi.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BasketApi.Infrastructure;

public static class Dependencies
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ICachingService, CachingService>();
    }
}
