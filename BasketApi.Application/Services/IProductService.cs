using BasketApi.Domain;

namespace BasketApi.Application.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetTopRankedProducts();
    Task<IEnumerable<Product>> GetPaginatedProducts(int pageSize, int pageNumber);
    Task<IEnumerable<Product>> GetCheapestProducts();
}
