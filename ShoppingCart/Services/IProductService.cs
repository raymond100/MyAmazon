using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(int productId);
        Task<List<Product>> GetProductsByCategorySlugAsync(string categorySlug);
        Task<Category> GetCategoryBySlugAsync(string slug);
        Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);

    }
}