using ShoppingCart.Models;
using ShoppingCart.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;

        public CartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<CartItem> GetByIdAsync(int id)
        {
            return await _cartItemRepository.GetByIdAsync(id);
        }

        public async Task<List<CartItem>> GetByUserIdAsync(int userId)
        {
            return await _cartItemRepository.GetByUserIdAsync(userId);
        }

        public async Task<CartItem> CreateAsync(CartItem cartItem)
        {
            return await _cartItemRepository.CreateAsync(cartItem);
        }

        public async Task<CartItem> UpdateAsync(CartItem cartItem)
        {
            return await _cartItemRepository.UpdateAsync(cartItem);
        }

        public async Task DeleteAsync(int id)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(id);
            if (cartItem != null)
            {
                await _cartItemRepository.DeleteAsync(cartItem);
            }
        }
    }
}
