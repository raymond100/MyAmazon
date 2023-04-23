using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCart.Models;
using ShoppingCart.Repository.BankSystem.BankSystemModels;

namespace ShoppingCart.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();

        Task<List<Product>> GetAllApprovedProductsAsync();
        Task<List<Product>> GetAllNonApprovedProductsAsync();

        Status ApproveProduct(long productId);
        Task<Product> GetProductByIdAsync(long productId);
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(long productId);
        Task<List<Product>> GetProductsByCategorySlugAsync(string slug);
        Task<List<Product>> GetProductsByCategoryIdAsync(long categoryId);
        Task<List<Product>> GetAllProductsAsyncByVendorId(String vendorId);
    }
}
