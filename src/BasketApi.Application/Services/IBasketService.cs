using BasketApi.Domain;

namespace BasketApi.Application.Services;

public interface IBasketService
{
    Basket? GetBasket(Guid baskedId);
    Task<Basket?> AddProductToBasket(Guid baskedId, BasketItem product);
    Basket? RemoveProductFromBasket(Guid baskedId, BasketItem product);
    Task<string?> CheckoutBasket(Guid baskedId);
    Guid CreateBasket();
    void ClearBasket(Guid basketId);
}
