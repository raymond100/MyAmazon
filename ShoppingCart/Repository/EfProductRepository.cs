using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;
using ShoppingCart.Repository.BankSystem.BankSystemModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Repository
{
    public class EfProductRepository : IProductRepository
    {
        private readonly ILogger<EfProductRepository> _logger;
        private readonly DataContext _dbContext;

        public EfProductRepository(DataContext context, ILogger<EfProductRepository> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _dbContext.Products.Include(p => p.Category).ToListAsync();
        }
        public Status ApproveProduct(long productId)
        {

            Product product = _dbContext.Products.Single(u => u.Id.Equals(productId));
            product.IsApproved = true;
            _dbContext.SaveChanges();
            Status status = new Status();
            status.StatusCode = 1;
            status.Message = "Product Approved";
            return status;

        }

        public async Task<List<Product>> GetAllApprovedProductsAsync()
        {
            return await _dbContext.Products.Where(p => p.IsApproved == true).ToListAsync();
        }
        public async Task<List<Product>> GetAllNonApprovedProductsAsync()
        {
            return await _dbContext.Products.Where(p => p.IsApproved == false).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(long productId)
        {
            _logger.LogInformation($"GetProductByIdAsync called with id: {productId}");
            return await _dbContext.Products.FindAsync(productId);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProductAsync(long productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Product>> GetProductsByCategorySlugAsync(string slug)
            {
                var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
                return await _dbContext.Products.Where(p => p.CategoryId == category.Id).ToListAsync();
            }

        public async Task<List<Product>> GetProductsByCategoryIdAsync(long categoryId)
        {
            return await _dbContext.Products
                .Where(p => (p.CategoryId == categoryId) && (p.IsApproved == true))
                .ToListAsync();

        }

    }
}
