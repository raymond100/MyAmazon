using ShoppingCart.Repository;
using ShoppingCart.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart.Services
{
   public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(long id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task<List<Product>> GetProductsByCategoryIdAsync(long categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
            return products.ToList();
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            return await _productRepository.AddProductAsync(product);
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            return await _productRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(long id)
        {
            await _productRepository.DeleteProductAsync(id);
        }

        public async Task<Category> GetCategoryBySlugAsync(string slug)
        {
            return await _categoryRepository.GetBySlugAsync(slug);
        }

        // public async Task<List<Product>> GetAllFeaturedProductsAsync()
        // {
        //     var products = await _productRepository.GetAllProductsAsync();
        //     return products.Where(p => p.IsFeatured).ToList();
        // }

        public async Task<List<Product>> SearchProductsAsync(string query)
        {
            var products = await _productRepository.GetAllProductsAsync();
            return products.Where(p => p.Name.ToLower().Contains(query.ToLower())).ToList();
        }

        public async Task<List<Product>> GetProductsByCategorySlugAsync(string categorySlug)
        {
            var category = await _categoryRepository.GetBySlugAsync(categorySlug);
            if (category == null)
            {
                return null;
            }

            var products = await _productRepository.GetProductsByCategoryIdAsync((long)category.Id);
            return products;
        }

    }

}