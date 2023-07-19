using BasketApi.Domain;

namespace BasketApi.Application.Services;

public interface IBasketService
{
    Basket GetBasket(Guid baskedId);
    Task AddProductToBasket(Guid baskedId, BasketItem product);
    Task RemoveProductFromBasket(Guid baskedId, BasketItem product);
    Task<Basket> CheckoutBasket(Guid baskedId);
}
