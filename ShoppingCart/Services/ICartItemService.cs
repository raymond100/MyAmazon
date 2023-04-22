using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public interface ICartItemService
    {
        Task<CartItem> GetByIdAsync(int id);
        Task<List<CartItem>> GetByUserIdAsync(int userId);
        Task<CartItem> CreateAsync(CartItem cartItem);
        Task<CartItem> UpdateAsync(CartItem cartItem);
        Task DeleteAsync(int id);
    }

}