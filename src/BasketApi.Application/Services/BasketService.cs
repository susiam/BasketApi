using BasketApi.Domain;
using BasketApi.Infrastructure.ApiClients;
using BasketApi.Infrastructure.Services;

namespace BasketApi.Application.Services;

public class BasketService : IBasketService
{
    private readonly ICachingService _cache;
    private readonly IProductService _productService;
    private readonly IProductApiClient _productApiClient;

    public BasketService(ICachingService cache,
                        IProductService productService,
                         IProductApiClient productApiClient)
    {
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(productService);
        ArgumentNullException.ThrowIfNull(productApiClient);
        _cache = cache;
        _productService = productService;
        _productApiClient = productApiClient;
    }

    public async Task<Basket?> AddProductToBasket(Guid baskedId, BasketItem item)
    {
        var basket = _cache.Get<Basket>(baskedId.ToString());

        if (basket != null && basket!.OrderLines.Any())
        {
            var productLineIndex = basket.OrderLines.FindIndex(o => o.ProductId == item.ProductId);
            if (productLineIndex != -1)
            {
                UpdateExistentItem(basket, productLineIndex, item.Quantity);
            }
            else
            {
                await AddNewItem(basket, item.ProductId, item.Quantity);
            }
            _cache.Set(baskedId.ToString(), basket);
            return basket;
        }

        return null;
    }

    public Basket? RemoveProductFromBasket(Guid baskedId, BasketItem product)
    {
        var basket = _cache.Get<Basket>(baskedId.ToString());
        if (basket?.OrderLines != null)
        {
            var productLineIndex = basket.OrderLines.FindIndex(o => o.ProductId == product.ProductId);
            if (productLineIndex != -1)
            {
                var productLine = basket.OrderLines[productLineIndex];
                double aumontToBeUpdated = productLine.ProductUnitPrice * product.Quantity;

                if (product.Quantity < basket.OrderLines[productLineIndex].Quantity)
                {
                    productLine.TotalPrice -= aumontToBeUpdated;
                    basket.OrderLines[productLineIndex] = productLine;
                    basket.TotalAmount -= aumontToBeUpdated;
                }
                else
                {
                    basket.OrderLines.RemoveAt(productLineIndex);
                    basket.TotalAmount -= aumontToBeUpdated;
                }
            }
            _cache.Set(baskedId.ToString(), basket);

            return basket;
        }

        return null;
    }

    public async Task<Basket?> CheckoutBasket(Guid baskedId)
    {
        var basket = _cache.Get<Basket>(baskedId.ToString());
        if (basket != null)
        {
            var finalOrder = new CreateOrderRequest()
            {
                TotalAmount = basket.TotalAmount,
                OrderLines = basket.OrderLines
            };
            var createOrder = await _productApiClient.CreateOrder(finalOrder);
            _cache.Remove(baskedId.ToString());
            return basket;
        }
        return null; 
    }

    public Basket GetBasket(Guid basketId)
    {
        return _cache.Get<Basket>(basketId.ToString());
    }

    public Guid CreateBasket()
    {
        var basket = new Basket();
        _cache.Set(basket.BasketId.ToString(), basket);

        return basket.BasketId;
    }

    public void ClearBasket(Guid basketId)
    {
        _cache.Remove(basketId.ToString());
    }

    private static void UpdateExistentItem(Basket basket, int productLineIndex, int quantity)
    {
        var productLine = basket.OrderLines[productLineIndex];
        double aumontToBeUpdated = productLine.ProductUnitPrice * quantity;

        productLine.TotalPrice += aumontToBeUpdated;

        basket.OrderLines[productLineIndex] = productLine;
        basket.TotalAmount += aumontToBeUpdated;
    }

    private async Task AddNewItem(Basket basket, int productId, int quantity)
    {
        var productDetails = await _productService.GetProductById(productId);
        if (productDetails == null)
            throw new Exception(); //TODO: add custom exception

        double aumontToBeUpdated = productDetails.Price * quantity;

        var productLine = new OrderLine()
        {
            ProductId = productDetails.Id,
            ProductSize = productDetails.Size.ToString(),
            Quantity = quantity,
            TotalPrice = aumontToBeUpdated
        };

        basket.TotalAmount += aumontToBeUpdated;
        basket.OrderLines.Add(productLine);
    }
}