using BasketApi.Domain;
using BasketApi.Infrastructure.ApiClients;
using BasketApi.Infrastructure.Services;
using Moq.Protected;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BasketApi.Unit.Tests.Infrastructure.Services
{
    public sealed class ProductsApiClientTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<ICachingService> _cacheServiceMock;
        private readonly ProductApiClient _productApiClient;

        public ProductsApiClientTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object);
            _cacheServiceMock = new Mock<ICachingService>();
            _productApiClient = new ProductApiClient(_httpClient, _cacheServiceMock.Object);
        }

        [Fact]
        public async Task CreateOrder_CreatesOrderSuccessfully()
        {
            // Arrange
            var orderRequest = new CreateOrderRequest();
            var order = new Order();
            var fakeHttpMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json")
            };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(fakeHttpMessage);

            // Act
            var result = await _productApiClient.CreateOrder(orderRequest);

            // Assert
            Assert.NotNull(result);
            // Add more assertions based on your expected outcome
        }

        [Fact]
        public async Task GetAllProductsCached_GetsAllProductsSuccessfully()
        {
            // Arrange
            var products = new List<Product>();
            _cacheServiceMock.Setup(c => c.GetOrAddAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<Product>>>>())).ReturnsAsync(products);

            // Act
            var result = await _productApiClient.GetAllProductsCached();

            // Assert
            Assert.NotNull(result);
            // Add more assertions based on your expected outcome
        }

        [Fact]
        public async Task GetAllProducts_GetsAllProductsSuccessfully()
        {
            // Arrange
            var products = new List<Product>();
            var fakeHttpMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(products), Encoding.UTF8, "application/json")
            };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(fakeHttpMessage);

            // Act
            var result = await _productApiClient.GetAllProducts();

            // Assert
            Assert.NotNull(result);
            // Add more assertions based on your expected outcome
        }

        [Fact]
        public async Task GetOrder_GetsOrderSuccessfully()
        {
            // Arrange
            var order = new Order();
            var fakeHttpMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json")
            };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(fakeHttpMessage);

            // Act
            var result = await _productApiClient.GetOrder("orderId");

            // Assert
            Assert.NotNull(result);
            // Add more assertions based on your expected outcome
        }
    }
}
