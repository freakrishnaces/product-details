using Microsoft.AspNetCore.Mvc;
using ProductModule.Interfaces;
using ProductModule.Models;

namespace ProductModule.Controllers
{
    
    [ApiController]
    [Route("[Controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        [Route("/productCategories")]
        public async Task<ActionResult<List<ProductCategory>>> ProductCategories()
        {
            try
            {
                var products = await _productService.GetAllProductCategoriesAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all products");
                throw;
            }
        }

        [HttpGet]
        [Route("/products")]
        public async Task<ActionResult<List<Product>>> GetProductsByCategory(string categoryId)
        {
            try
            {
                var products = await _productService.GetProductsByCategoryAsync(categoryId);
                return Ok(products);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error");
                throw;
            }
        }
        
    }
}
