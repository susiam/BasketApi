using BasketApi.Domain;
using BasketApi.Infrastructure.ApiClients;

namespace BasketApi.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductApiClient _productApiClient;

    public ProductService(IProductApiClient productApiClient)
    {
        ArgumentNullException.ThrowIfNull(productApiClient);
        _productApiClient = productApiClient;
    }

    public async Task<IEnumerable<Product>> GetTopRankedProducts()
    {
        var allProducts = await _productApiClient.GetAllProducts();

        return allProducts.OrderByDescending(p => p.Stars).OrderBy(p => p.Price).Take(100);
    }

    public async Task<IEnumerable<Product>> GetPaginatedProducts(int pageSize, int pageNumber)
    {
        return (await _productApiClient.GetAllProducts()).OrderBy(p => p.Price).Skip(pageSize * pageNumber).Take(pageSize);
    }

    public async Task<IEnumerable<Product>> GetCheapestProducts()
    {
        return (await _productApiClient.GetAllProducts()).OrderBy(p => p.Price).Take(10);
    }

    public async Task<Product> GetProductById(int id)
    {
        return (await _productApiClient.GetAllProducts()).FirstOrDefault(p => p.Id == id);
    }
}
