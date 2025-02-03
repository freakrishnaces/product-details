using ProductModule.Models;

namespace ProductModule.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductCategory>> GetAllProductCategoriesAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryId);
    }
}