using BasketApi.Domain;
using BasketApi.Infrastructure.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BasketApi.Infrastructure.ApiClients;

public class ProductApiClient : IProductApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ICachingService _cacheService;

    public ProductApiClient(HttpClient httpClient,
                           ICachingService cacheService)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(cacheService);
        _httpClient = httpClient;
        _cacheService = cacheService;
    }

    public async Task<Order> CreateOrder(CreateOrder order)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetApiToken());

        var response = await _httpClient.PostAsJsonAsync("api/CreateOrder", order);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Order>();
    }

    public async Task<IEnumerable<Product>> GetAllProductsCached()
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetApiToken());

        return await _cacheService.GetOrAddAsync("products", () =>
         this.GetAllProducts()
         );
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetApiToken());

        var response = await _httpClient.GetAsync("api/GetAllProducts");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
    }

    public async Task<Order> GetOrder(string orderId)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetApiToken());

        var response = await _httpClient.GetAsync($"api/GetOrder/{orderId}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Order>();
    }

    private async Task<string> GetApiToken()
    {
        var requestBody = new { email = "client@mail.com" };

        var response = await _httpClient.PostAsJsonAsync("api/login", requestBody);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<ApiToken>()).Token;
    }
}
