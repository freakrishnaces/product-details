using Microsoft.AspNetCore.Mvc;
using ProductModule.Interfaces;
using ProductModule.Models;

namespace ProductModule.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AddProductsController : ControllerBase
    {
        private readonly ILogger<AddProductsController> _logger;
        private readonly IAddProductsService _addProductService;
        public AddProductsController(ILogger<AddProductsController> logger, IAddProductsService addProductsService)
        {
            _logger = logger;
            _addProductService = addProductsService;
        }

        [HttpPost]
        [Route("/addNewProductCategory")]
        public async Task<ActionResult<ProductCategory>> AddNewProductCategory(string categoryName)
        {
            var category = await _addProductService.AddProductCategoryAsync(categoryName);
            return Ok(category);
        }

        [HttpPost]
        [Route("/addProductsToCategory")]
        public async Task<IActionResult> AddProductsToCategory([FromBody] List<Product> products)
        {
            var result = await _addProductService.AddProductsToCategoryAsync(products);
            return Ok(result);
        }

        [HttpPost]
        [Route("/addProductDetails")]
        public async Task<ActionResult<List<ProductSpecifications>>> AddProductSpecifications([FromBody] List<ProductSpecifications> productSpecs)
        {
            var result = await _addProductService.AddProductSpecificationsAsync(productSpecs);
            return Ok(true);
        }
    }
}