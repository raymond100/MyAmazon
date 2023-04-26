using ShoppingCart.Repository;
using ShoppingCart.Models;
using Microsoft.EntityFrameworkCore;

namespace  ShoppingCart.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Category> GetBySlugAsync(string slug)
        {
            return await _categoryRepository.GetBySlugAsync(slug);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            return await _categoryRepository.CreateAsync(category);
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            return await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteAsync(Category category)
        {
            await _categoryRepository.DeleteAsync(category);
        }

        public Task<List<Category>> GetApprovedCategoriesAsync()
        {
            return _categoryRepository.GetApprovedCategoriesAsync();
        }

        public Task<List<Category>> GetNonApprovedCategoriesAsync()
        {
            return _categoryRepository.GetNonApprovedCategoriesAsync();
        }
    }
}