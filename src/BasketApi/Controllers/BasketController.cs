using BasketApi.Application.Services;
using BasketApi.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BasketApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost("create")]
        [ProducesResponseType(200)]
        public IActionResult CreateBasket()
        {
            var basketId = _basketService.CreateBasket();

            return Ok(basketId);
        }

        [HttpGet("{basketId}")]
        [ProducesResponseType(200, Type = typeof(Basket))]
        [ProducesResponseType(404)]
        public IActionResult GetBasket(Guid basketId)
        {
            var basket = _basketService.GetBasket(basketId);
            return basket == default ? NotFound(basketId) : Ok(basket);
        }

        [HttpPost("{basketId}/products")]
        [ProducesResponseType(200, Type = typeof(Basket))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddProduct(Guid basketId, BasketItem product)
        {
            var basket = await _basketService.AddProductToBasket(basketId, product);
             return basket == default ? NotFound(basketId) : Ok(basket);
        }

        [HttpDelete("{basketId}/products/{productId}")]
        [ProducesResponseType(200, Type = typeof(Basket))]
        [ProducesResponseType(404)]
        public IActionResult RemoveProduct(Guid basketId, BasketItem product)
        {
            var basket = _basketService.RemoveProductFromBasket(basketId, product);
             return basket == default ? NotFound(basketId) : Ok(basket);
        }

        [HttpPost("{basketId}/submit")]
        [ProducesResponseType(200, Type = typeof(Basket))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CheckoutBasket(Guid basketId)
        {
            await _basketService.CheckoutBasket(basketId);
            return Ok();
        }

        [HttpDelete("{basketId}")]
        [ProducesResponseType(200, Type = typeof(Basket))]
        public IActionResult ClearBasket(Guid basketId)
        {
            _basketService.ClearBasket(basketId);
            return Ok();
        }
    }
}
