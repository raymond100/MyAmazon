using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCart.Models;
using ShoppingCart.Repository.BankSystem.BankSystemModels;

namespace ShoppingCart.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();

        Task<List<Product>> GetAllApprovedProductsAsync();
        Task<List<Product>> GetAllNonApprovedProductsAsync();
        Status ApproveProduct(long productId);

        Task<Product> GetProductByIdAsync(long productId);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(long productId);
        Task<List<Product>> GetProductsByCategorySlugAsync(string categorySlug);
        Task<Category> GetCategoryBySlugAsync(string slug);
        Task<List<Product>> GetProductsByCategoryIdAsync(long categoryId);
        Task<List<Product>> GetAllProductsAsyncByVendorId(String vendorId);
    }
}