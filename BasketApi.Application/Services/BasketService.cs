using BasketApi.Domain;
using BasketApi.Infrastructure.ApiClients;
using BasketApi.Infrastructure.Services;

namespace BasketApi.Application.Services;

public class BasketService : IBasketService
{
    private readonly ICachingService _cache;
    private readonly IProductApiClient _productApiClient;

    public BasketService(ICachingService cache,
                         IProductApiClient productApiClient)
    {
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(productApiClient);
        _cache = cache;
        _productApiClient = productApiClient;
    }

    //this method could be simplified
    public async Task AddProductToBasket(Guid baskedId, BasketItem product)
    {
        var basket = _cache.Get<Basket>(baskedId.ToString());
        double aumontToBeUpdated = product.Price * product.Quantity;
        if (basket?.OrderLines != null)
        {
            var productLineIndex = basket.OrderLines.FindIndex(o => o.ProductId == product.Id);
            if (productLineIndex != -1)
            {
                var productLine = basket.OrderLines[productLineIndex];

                productLine.TotalPrice += aumontToBeUpdated;
                basket.OrderLines[productLineIndex] = productLine;
                basket.TotalAmount += aumontToBeUpdated;
            }
            else
            {
                var productLine = new OrderLine()
                {
                    ProductId = product.Id,
                    ProductSize = product.Size.ToString(),
                    Quantity = product.Quantity,
                    TotalPrice = aumontToBeUpdated
                };
                 basket.TotalAmount += aumontToBeUpdated;
                basket.OrderLines.Add(productLine);
            }
        }
        else
        {
            basket = new();
            basket.BasketId = Guid.NewGuid();
            basket.OrderLines = new();
            var productLine = new OrderLine()
            {
                ProductId = product.Id,
                ProductSize = product.Size.ToString(),
                Quantity = product.Quantity

            };
            basket.OrderLines.Add(productLine);
        }

        _cache.Set(baskedId.ToString(), basket);
    }

    public async Task RemoveProductFromBasket(Guid baskedId, BasketItem product)
    {
        var basket = _cache.Get<Basket>(baskedId.ToString());
        if (basket?.OrderLines != null)
        {
            var productLineIndex = basket.OrderLines.FindIndex(o => o.ProductId == product.Id);
            if (productLineIndex != -1)
            {
                if (product.Quantity < basket.OrderLines[productLineIndex].Quantity)
                {
                    var productLine = basket.OrderLines[productLineIndex];
                    double aumontToBeUpdated = product.Price * product.Quantity;
                    productLine.TotalPrice -= aumontToBeUpdated;
                    basket.OrderLines[productLineIndex] = productLine;
                    basket.TotalAmount -= aumontToBeUpdated;
                }
                else
                {
                    basket.OrderLines.RemoveAt(productLineIndex);
                }
            }
        }

        _cache.Set(baskedId.ToString(), basket);
    }

    public async Task<Basket> CheckoutBasket(Guid baskedId)
    {
        var basket = _cache.Get<Basket>(baskedId.ToString());
        if (basket != null)
        {
            var finalOrder = new CreateOrder()
            {
                UserEmail = basket!.UserEmail,
                TotalAmount = basket.TotalAmount,
                OrderLines = basket.OrderLines
            };
            var createOrder = await _productApiClient.CreateOrder(finalOrder);
            _cache.Remove(baskedId.ToString());
            return basket;
        }
        return null; // or throw an exception
    }

    public Basket GetBasket(Guid baskedId)
    {
        return _cache.Get<Basket>(baskedId.ToString());
    }
}