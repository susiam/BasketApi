using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace BasketApi.Integration.Tests.Controllers;

public sealed class BasketControllerIntegrationTests
{
    [Fact]
    public async Task GetBasket_ReturnsNotFound_WhenBasketDoesNotExist()
    {
        await using var application = new WebApplicationFactory<Program>();
        // Arrange
        var client = application.CreateClient();
        var basketId = Guid.NewGuid();

        // Act
        var response = await client.GetAsync($"/api/basket/{basketId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetBasket_ReturnsOK_WhenBasketExist()
    {
        await using var application = new WebApplicationFactory<Program>();
        // Arrange
        var client = application.CreateClient();

        // Act
        var basketId = await client.PostAsync($"/api/basket/create", null);
        var response = await client.GetAsync($"/api/basket/{basketId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CheckoutBasket_ReturnsNotFound_WhenBasketDoesNotExist()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();
        var basketId = Guid.NewGuid();

        // Act
        var response = await client.PostAsync($"/api/basket/{basketId}/submit", null);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

}
