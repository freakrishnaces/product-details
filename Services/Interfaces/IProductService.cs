using ProductModule.Models;

namespace ProductModule.Interfaces
{
    public interface IProductService 
    {
        Task<List<ProductCategory>> GetAllProductCategoriesAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryId);
    }
}