using BasketApi.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BasketApi.Application;

public static class Dependencies
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<IProductService, ProductService>();
    }
}