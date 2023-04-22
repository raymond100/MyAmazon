
using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public interface ICartService
    {
        Task<List<CartItem>> GetCartItemsAsync();
        // Task SaveCartItemsAsync(List<CartItem> cartItems);
        Task ClearCartAsync();
        Task UpdateCartItemAsync(int productId, int quantity);
        Task AddCartItemAsync(Product product);
        Task DecreaseCartItemAsync(int productId);
        Task RemoveCartItemAsync(int productId);
    }
}