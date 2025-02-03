using ProductModule.Interfaces;
using ProductModule.Models;

namespace ProductModule.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<ProductCategory>> GetAllProductCategoriesAsync()
        {
            return await _repository.GetAllProductCategoriesAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryId)
        {
            return await _repository.GetProductsByCategoryAsync(categoryId);
        }
    }
}