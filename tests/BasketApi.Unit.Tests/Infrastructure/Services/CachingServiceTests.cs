using BasketApi.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace BasketApi.Unit.Tests.Infrastructure.Services;

public sealed class CachingServiceTests
{
    private readonly Mock<IMemoryCache> _cacheMock;
    private readonly CachingService _cachingService;

    public CachingServiceTests()
    {
        _cacheMock = new Mock<IMemoryCache>();
        _cachingService = new CachingService(_cacheMock.Object);
    }

    [Fact]
    public async Task GetOrAddAsync_GetsFromCache_WhenDataExists()
    {
        // Arrange
        _cacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny)).Returns(true);

        // Act
        var result = await _cachingService.GetOrAddAsync("testKey", () => Task.FromResult("testData"));

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetOrAddAsync_AddsToCache_WhenDataNotExists()
    {
        // Arrange
        _cacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny)).Returns(false);

        // Act
        var result = await _cachingService.GetOrAddAsync("testKey", () => Task.FromResult("testData"));

        // Assert
        _cacheMock.Verify(x => x.Set(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
    }

    [Fact]
    public void Get_ReturnsData_WhenDataExists()
    {
        // Arrange
        _cacheMock.Setup(x => x.Get<string>(It.IsAny<object>())).Returns("testData");

        // Act
        var result = _cachingService.Get<string>("testKey");

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Set_AddsDataToCache()
    {
        // Act
        _cachingService.Set("testKey", "testData");

        // Assert
        _cacheMock.Verify(x => x.Set(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
    }

    [Fact]
    public void Remove_DeletesDataFromCache()
    {
        // Act
        _cachingService.Remove("testKey");

        // Assert
        _cacheMock.Verify(x => x.Remove(It.IsAny<object>()), Times.Once);
    }
}