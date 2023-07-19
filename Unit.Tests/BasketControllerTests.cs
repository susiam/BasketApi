using AutoFixture;
using BasketApi.Application.Services;
using BasketApi.Controllers;
using BasketApi.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Unit.Tests;

public class BasketControllerTests
{
    private readonly Mock<IBasketService> _mockBasketService;
    private readonly BasketController _controller;

    public BasketControllerTests()
    {
        _mockBasketService = new Mock<IBasketService>();
        _controller = new BasketController(_mockBasketService.Object);
    }

    [Fact]
    public async Task GetBasket_ReturnsOkResultWithBasket()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var expectedBasket = new Basket { BasketId = basketId };
        _mockBasketService.Setup(service => service.GetBasket(basketId)).Returns(expectedBasket);

        // Act
        var result = await _controller.GetBasket(basketId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualBasket = Assert.IsType<Basket>(okResult.Value);
        Assert.Equal(expectedBasket, actualBasket);
    }

    [Fact]
    public async Task AddProduct_ReturnsOkResult()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var product = new BasketItem { Id = 1, Name = "Product 1", Size = 10, Price = 9.99, Stars = 4, Quantity = 1 };

        // Act
        var result = await _controller.AddProduct(basketId, product);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        _mockBasketService.Verify(service => service.AddProductToBasket(basketId, product), Times.Once);
    }

    [Fact]
    public async Task RemoveProduct_ReturnsOkResult()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var product = new BasketItem { Id = 1, Name = "Product 1", Size = 10, Price = 9.99, Stars = 3, Quantity = 1 };

        // Act
        var result = await _controller.RemoveProduct(basketId, product);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        _mockBasketService.Verify(service => service.RemoveProductFromBasket(basketId, product), Times.Once);
    }

    [Fact]
    public async Task CheckoutBasket_ReturnsOkResult()
    {
        // Arrange
        var basketId = Guid.NewGuid();

        // Act
        var result = await _controller.CheckoutBasket(basketId);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        _mockBasketService.Verify(service => service.CheckoutBasket(basketId), Times.Once);
    }
}