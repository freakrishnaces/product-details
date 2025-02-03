using ProductModule.Models;

namespace ProductModule.Interfaces
{
    public interface IAddProductsService
    {
        Task<ProductCategory> AddProductCategoryAsync(string categoryName);
        Task<bool> AddProductsToCategoryAsync(List<Product> products);
    }
}