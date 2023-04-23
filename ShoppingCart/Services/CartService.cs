using ShoppingCart.Models;
using ShoppingCart.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.Services
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
       
        public CartService(DataContext context, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<CartItem>> GetCartItemsAsync()
        {
            var cartItems = new List<CartItem>();

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                if(user == null)
                {
                    return cartItems;
                }
                Cart userCart = await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product)
                                    .FirstOrDefaultAsync(c => c.UserId == user.Id);

                if (userCart == null)
                {
                    return cartItems;
                }

                cartItems = userCart.CartItems;
            }
            else
            {
                string sessionCartItemsJson = _httpContextAccessor.HttpContext.Session.GetString("Cart");
                if (sessionCartItemsJson != null)
                {
                    cartItems = JsonConvert.DeserializeObject<List<CartItem>>(sessionCartItemsJson);
                }
            }

            return cartItems;
        }

         public async Task AddCartItemAsync(Product product)
        {
            var cartItems = await GetCartItemsAsync();

            var existingCartItem = cartItems.FirstOrDefault(ci => ci.Product.Id == product.Id);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity++;
            }
            else
            {
                cartItems.Add(new CartItem { Product = product, Quantity = 1 });
            }

            await SaveCartItemsAsync(cartItems);
        }

        public async Task UpdateCartItemAsync(int productId, int quantity)
        {
            var cartItems = await GetCartItemsAsync();

            var existingCartItem = cartItems.FirstOrDefault(ci => ci.Product.Id == productId);

            if (existingCartItem != null)
            {
                if (quantity == 0)
                {
                    cartItems.Remove(existingCartItem);
                }
                else
                {
                    existingCartItem.Quantity = quantity;
                }

                await SaveCartItemsAsync(cartItems);
            }
        }

        public async Task DecreaseCartItemAsync(int productId)
        {
            var cartItems = await GetCartItemsAsync();

            var existingCartItem = cartItems.FirstOrDefault(ci => ci.Product.Id == productId);

            if (existingCartItem != null)
            {
                if (existingCartItem.Quantity == 1)
                {
                    cartItems.Remove(existingCartItem);
                }
                else
                {
                    existingCartItem.Quantity -= 1;
                }

                await SaveCartItemsAsync(cartItems);
            }
        }

        public async Task RemoveCartItemAsync(int productId)
        {
            var cartItems = await GetCartItemsAsync();

            var existingCartItem = cartItems.FirstOrDefault(ci => ci.Product.Id == productId);

            if (existingCartItem != null)
            {
                cartItems.Remove(existingCartItem);
                await SaveCartItemsAsync(cartItems);
            }
        }

        public async Task ClearCartAsync()
        {
            var cartItems = await GetCartItemsAsync();

            cartItems.Clear();

            await SaveCartItemsAsync(cartItems);
        }

        private async Task SaveCartItemsAsync(List<CartItem> cartItems)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.GetUserAsync(httpContext.User);

                Cart userCart = await _context.Carts.Include(c => c.CartItems)
                                            .FirstOrDefaultAsync(c => c.UserId == user.Id);

                if (userCart == null)
                {
                    userCart = new Cart { UserId = user.Id };
                    _context.Carts.Add(userCart);
                }

                userCart.CartItems = cartItems;
            }
            else
            {
                var sessionCartItemsJson = JsonConvert.SerializeObject(cartItems);
                httpContext.Session.SetString("Cart", sessionCartItemsJson);
            }

            await _context.SaveChangesAsync();
        }
    }

    
}
