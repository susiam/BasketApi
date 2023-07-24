using AutoFixture;
using BasketApi.Application.Services;
using BasketApi.Controllers;
using BasketApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BasketApi.Unit.Tests;

public sealed class BasketControllerTests
{
    private readonly Mock<IBasketService> _basketServiceMock;
    private readonly BasketController _basketController;
    private readonly Fixture _fixture;

    public BasketControllerTests()
    {
        _basketServiceMock = new Mock<IBasketService>();
        _basketController = new BasketController(_basketServiceMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public void CreateBasket_ReturnsBasketId()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        _basketServiceMock.Setup(bs => bs.CreateBasket()).Returns(basketId);

        // Act
        var result = _basketController.CreateBasket();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(basketId, okResult.Value);
    }

    [Fact]
    public void GetBasket_ReturnsBasket_WhenBasketExists()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var basket = new Basket();
        _basketServiceMock.Setup(bs => bs.GetBasket(basketId)).Returns(basket);

        // Act
        var result = _basketController.GetBasket(basketId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(basket, okResult.Value);
    }

    [Fact]
    public void GetBasket_ReturnsNotFound_WhenBasketDoesNotExist()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        _basketServiceMock.Setup(bs => bs.GetBasket(basketId)).Returns(default(Basket));

        // Act
        var result = _basketController.GetBasket(basketId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task AddProduct_ReturnsBasket_WhenBasketExists()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var basket = new Basket();
        var basketItem = new BasketItem();
        _basketServiceMock.Setup(bs => bs.AddProductToBasket(basketId, basketItem)).ReturnsAsync(basket);

        // Act
        var result = await _basketController.AddProduct(basketId, basketItem);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(basket, okResult.Value);
    }

    [Fact]
    public async Task AddProduct_ReturnsNotFound_WhenBasketDoesNotExist()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var basketItem = new BasketItem();
        _basketServiceMock.Setup(bs => bs.AddProductToBasket(basketId, basketItem)).ReturnsAsync(default(Basket));

        // Act
        var result = await _basketController.AddProduct(basketId, basketItem);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void RemoveProduct_ReturnsBasket_WhenBasketExists()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var basket = new Basket();
        var basketItem = new BasketItem();
        _basketServiceMock.Setup(bs => bs.RemoveProductFromBasket(basketId, basketItem)).Returns(basket);

        // Act
        var result = _basketController.RemoveProduct(basketId, basketItem);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(basket, okResult.Value);
    }

    [Fact]
    public void RemoveProduct_ReturnsNotFound_WhenBasketDoesNotExist()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var basketItem = new BasketItem();
        _basketServiceMock.Setup(bs => bs.RemoveProductFromBasket(basketId, basketItem)).Returns(default(Basket));

        // Act
        var result = _basketController.RemoveProduct(basketId, basketItem);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task CheckoutBasket_ReturnsOk_WhenCheckoutIsSuccessful()
    {
        // Arrange
        var orderId = Guid.NewGuid().ToString();
        var basketId = Guid.NewGuid();
        _basketServiceMock.Setup(bs => bs.CheckoutBasket(basketId)).ReturnsAsync(orderId);

        // Act
        var result = await _basketController.CheckoutBasket(basketId);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void ClearBasket_ReturnsOk_WhenClearIsSuccessful()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        _basketServiceMock.Setup(bs => bs.ClearBasket(basketId));

        // Act
        var result = _basketController.ClearBasket(basketId);

        // Assert
        Assert.IsType<OkResult>(result);
    }
}
