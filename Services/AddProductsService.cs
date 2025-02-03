using ProductModule.Interfaces;
using ProductModule.Models;

namespace ProductModule.Services
{
    public class AddProductsService : IAddProductsService
    {
        private readonly IAddProductsRepository _repository;
        public AddProductsService(IAddProductsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductCategory> AddProductCategoryAsync(string categoryName)
        {
            return await _repository.AddProductCategoryAsync(categoryName);
        }

        public async Task<bool> AddProductsToCategoryAsync(List<Product> products)
        {
            return await _repository.AddProductsToCategoryAsync(products);
        }
    }
}