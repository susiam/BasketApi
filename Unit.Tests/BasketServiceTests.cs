using BasketApi.Application.Services;
using BasketApi.Domain;
using BasketApi.Infrastructure.ApiClients;
using BasketApi.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Unit.Tests;


public class BasketServiceTests
{
        private readonly Mock<ICachingService> _mockMemoryCache;
    private readonly Mock<IProductApiClient> _mockProductApiClient;
    private readonly BasketService _basketService;

    public BasketServiceTests()
    {
        _mockMemoryCache = new Mock<ICachingService>();
        _mockProductApiClient = new Mock<IProductApiClient>();
        _basketService = new BasketService(_mockMemoryCache.Object, _mockProductApiClient.Object);
    }

    [Fact]
    public void GetBasket_ReturnsBasketFromCache()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var expectedBasket = new Basket { BasketId = basketId };
        _mockMemoryCache.Setup(cache => cache.Get<Basket>(basketId.ToString())).Returns(expectedBasket);

        // Act
        var result = _basketService.GetBasket(basketId);

        // Assert
        Assert.Equal(expectedBasket, result);
    }

    [Fact]
    public async Task AddProductToBasket_AddsProductToExistingBasket()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var existingBasket = new Basket { BasketId = basketId, OrderLines = new List<OrderLine>() };
        var product = new BasketItem { Id = 1, Size = 10, Quantity = 2, Price = 5.99 };
        _mockMemoryCache.Setup(cache => cache.Get<Basket>(basketId.ToString())).Returns(existingBasket);

        // Act
        await _basketService.AddProductToBasket(basketId, product);

        // Assert
        _mockMemoryCache.Verify(cache => cache.Set(basketId.ToString(), It.IsAny<Basket>()), Times.Once);
        Assert.Single(existingBasket.OrderLines);
        var addedProductLine = existingBasket.OrderLines[0];
        Assert.Equal(product.Id, addedProductLine.ProductId);
        Assert.Equal(product.Size.ToString(), addedProductLine.ProductSize);
        Assert.Equal(product.Quantity, addedProductLine.Quantity);
        Assert.Equal(product.Price * product.Quantity, addedProductLine.TotalPrice);
    }

    [Fact]
    public async Task AddProductToBasket_CreatesNewBasketAndAddsProduct()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var product = new BasketItem { Id = 1, Size = 10, Quantity = 2, Price = 5.99 };
        _mockMemoryCache.Setup(cache => cache.Get<Basket>(basketId.ToString())).Returns<Basket>(null);

        // Act
        await _basketService.AddProductToBasket(basketId, product);

        // Assert
        _mockMemoryCache.Verify(cache => cache.Set(basketId.ToString(), It.IsAny<Basket>()), Times.Once);
        _mockMemoryCache.Verify(cache => cache.Get<Basket>(basketId.ToString()), Times.Once);
    }

    [Fact]
    public async Task RemoveProductFromBasket_RemovesProductFromBasket()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var product = new BasketItem { Id = 1, Size = 10, Quantity = 2, Price = 5.99 };
        var existingBasket = new Basket
        {
            BasketId = basketId,
            OrderLines = new List<OrderLine>
            {
                new OrderLine
                {
                    ProductId = product.Id,
                    ProductSize = product.Size.ToString(),
                    Quantity = product.Quantity,
                    TotalPrice = product.Price * product.Quantity
                }
            }
        };
        _mockMemoryCache.Setup(cache => cache.Get<Basket>(basketId.ToString())).Returns(existingBasket);

        // Act
        await _basketService.RemoveProductFromBasket(basketId, product);

        // Assert
        _mockMemoryCache.Verify(cache => cache.Set(basketId.ToString(), It.IsAny<Basket>()), Times.Once);
        Assert.Empty(existingBasket.OrderLines);
    }

    [Fact]
    public async Task CheckoutBasket_ReturnsBasketAfterCheckout()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var basket = new Basket { BasketId = basketId, UserEmail = "test@example.com" };
        var createOrder = new CreateOrder { UserEmail = basket.UserEmail };
        _mockMemoryCache.Setup(cache => cache.Get<Basket>(basketId.ToString())).Returns(basket);

        // Act
        var result = await _basketService.CheckoutBasket(basketId);

        // Assert
        _mockMemoryCache.Verify(cache => cache.Remove(basketId.ToString()), Times.Once);
        Assert.Equal(basket, result);
    }
}