using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BasketApi.Unit.Tests.IntegrationTests.Controllers
{
public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public ProductsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTopProducts_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/products/top");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetPaginatedProducts_ReturnsSuccessStatusCode()
    {
        // Arrange
        var pageSize = 10;
        var pageNumber = 1;

        // Act
        var response = await _client.GetAsync($"/products/{pageSize}/{pageNumber}");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetPaginatedProducts_ReturnsBadRequestForPageSizeOver1000()
    {
        // Arrange
        var pageSize = 1001;
        var pageNumber = 1;

        // Act
        var response = await _client.GetAsync($"/products/{pageSize}/{pageNumber}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetCheapestProducts_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/products/cheapest");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
}
