using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;
using ShoppingCart.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Repository
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly DataContext _dbContext;

        public CartItemRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CartItem> GetByIdAsync(int id)
        {
            return await _dbContext.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.User)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }

        public async Task<List<CartItem>> GetByUserIdAsync(int userId)
        {
            return await _dbContext.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.User.Id == userId.ToString())
                .ToListAsync();
        }

        public async Task<CartItem> CreateAsync(CartItem cartItem)
        {
            _dbContext.CartItems.Add(cartItem);
            await _dbContext.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem> UpdateAsync(CartItem cartItem)
        {
            _dbContext.Entry(cartItem).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return cartItem;
        }

        public async Task DeleteAsync(CartItem cartItem)
        {
            _dbContext.CartItems.Remove(cartItem);
            await _dbContext.SaveChangesAsync();
        }

        public List<CartItem> GetCartItemsForUser(string userId)
        {
            return _dbContext.CartItems.Where(c => c.User.Id == userId).ToList();;
        }


        public CartViewModel GetCartViewModelForUser(string userId)
        {
            var cart = _dbContext.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _dbContext.Carts.Add(cart);
                _dbContext.SaveChanges();
            }

            return new CartViewModel(cart);
        }



    }
}
