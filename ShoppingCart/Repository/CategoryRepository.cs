using ShoppingCart.Models;
using ShoppingCart.Data;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            //return await _context.Categories.FindAsync(id);
            return await _context.Categories.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category> GetBySlugAsync(string slug)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetApprovedCategoriesAsync()
        {
            return await _context.Categories.Where(c => c.IsApproved == true).ToListAsync();
        }
        public async Task<List<Category>> GetNonApprovedCategoriesAsync()
        {
            return await _context.Categories.Where(c => c.IsApproved == false).ToListAsync();
        }
    }
}