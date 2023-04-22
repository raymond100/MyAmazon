using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(long productId);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(long productId);
        Task<List<Product>> GetProductsByCategorySlugAsync(string categorySlug);
        Task<Category> GetCategoryBySlugAsync(string slug);
        Task<List<Product>> GetProductsByCategoryIdAsync(long categoryId);

    }
}