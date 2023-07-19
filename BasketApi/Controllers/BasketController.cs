using BasketApi.Application.Services;
using BasketApi.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BasketApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("{basketId}")]
        public async Task<IActionResult> GetBasket(Guid basketId)
        {
            var basket = _basketService.GetBasket(basketId);
            return Ok(basket);
        }

        [HttpPost("{basketId}/products")]
        public async Task<IActionResult> AddProduct(Guid basketId, BasketItem product)
        {
            await _basketService.AddProductToBasket(basketId, product);
            return Ok();
        }

        [HttpDelete("{basketId}/products/{productId}")]
        public async Task<IActionResult> RemoveProduct(Guid basketId, BasketItem product)
        {
            await _basketService.RemoveProductFromBasket(basketId, product);
            return Ok();
        }

        [HttpPost("{basketId}/submit")]
        public async Task<IActionResult> CheckoutBasket(Guid basketId)
        {
            await _basketService.CheckoutBasket(basketId);
            return Ok();
        }
    }
}
