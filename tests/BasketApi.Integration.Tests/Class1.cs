using BasketApi.Application.Services;
using BasketApi.Controllers;
using BasketApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mockProductService = new Mock<IProductService>();
        _controller = new ProductsController(_mockProductService.Object);
    }

    [Fact]
    public async Task GetTopProducts_ReturnsOkResultWithProducts()
    {
        // Arrange
        var expectedProducts = new List<Product> { new Product { Name = "Product 1" }, new Product { Name = "Product 2" } };
        _mockProductService.Setup(service => service.GetTopRankedProducts()).ReturnsAsync(expectedProducts);

        // Act
        var result = await _controller.GetTopProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualProducts = Assert.IsType<List<Product>>(okResult.Value);
        Assert.Equal(expectedProducts, actualProducts);
    }

    [Fact]
    public async Task GetPaginatedProducts_ReturnsOkResultWithProducts()
    {
        // Arrange
        var pageSize = 10;
        var pageNumber = 1;
        var expectedProducts = new List<Product> { new Product { Name = "Product 1" }, new Product { Name = "Product 2" } };
        _mockProductService.Setup(service => service.GetPaginatedProducts(pageSize, pageNumber)).ReturnsAsync(expectedProducts);

        // Act
        var result = await _controller.GetPaginatedProducts(pageSize, pageNumber);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualProducts = Assert.IsType<List<Product>>(okResult.Value);
        Assert.Equal(expectedProducts, actualProducts);
    }

    [Fact]
    public async Task GetCheapestProducts_ReturnsOkResultWithProducts()
    {
        // Arrange
        var expectedProducts = new List<Product> { new Product { Name = "Product 1" }, new Product { Name = "Product 2" } };
        _mockProductService.Setup(service => service.GetCheapestProducts()).ReturnsAsync(expectedProducts);

        // Act
        var result = await _controller.GetCheapestProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualProducts = Assert.IsType<List<Product>>(okResult.Value);
        Assert.Equal(expectedProducts, actualProducts);
    }
}