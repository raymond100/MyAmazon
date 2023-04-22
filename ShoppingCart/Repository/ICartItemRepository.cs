using ShoppingCart.Models;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Repository
{
    public interface ICartItemRepository
    {
        Task<CartItem> GetByIdAsync(int id);
        Task<List<CartItem>> GetByUserIdAsync(int userId);
        Task<CartItem> CreateAsync(CartItem cartItem);
        Task<CartItem> UpdateAsync(CartItem cartItem);
        Task DeleteAsync(CartItem cartItem);
        CartViewModel GetCartViewModelForUser(string userId);
        List<CartItem> GetCartItemsForUser(string userId);

    }
}