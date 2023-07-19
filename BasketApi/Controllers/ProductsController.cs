using BasketApi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BasketApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("top")]
        public async Task<IActionResult> GetTopProducts()
        {
            var products = await _productService.GetTopRankedProducts();
            return Ok(products);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedProducts(int pageSize, int pageNumber)
        {
            if(pageSize > 1000)
                return BadRequest("Page size should be below 1000.");

            var products = await _productService.GetPaginatedProducts(pageSize, pageNumber);
            return Ok(products);
        }

        [HttpGet("cheapest")]
        public async Task<IActionResult> GetCheapestProducts()
        {
            var products = await _productService.GetCheapestProducts();
            return Ok(products);
        }
    }
}
