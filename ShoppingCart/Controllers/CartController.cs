using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;
using ShoppingCart.Services;
using ShoppingCart.Repository;
using ShoppingCart.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;


namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {

        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ITaxRateRepository _rateRepository;

        public CartController(
        DataContext context, 
        UserManager<AppUser> userManager, 
        IProductService productService,
        ICartService cartService,
        ITaxRateRepository rateRepository)
        {
            _context = context;
            _userManager = userManager;
            _productService = productService;
            _cartService = cartService;
            _rateRepository = rateRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<CartItem> cartItems;
            TaxRate rate = null;

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.GetUserAsync(User);
                Cart userCart = await _context.Carts.Include(c => c.CartItems)
                                     .ThenInclude(ci => ci.Product)
                                     .Include(c => c.Rate)
                                     .FirstOrDefaultAsync(c => c.UserId == user.Id);

                if (userCart == null)
                {
                    return View(new CartViewModel(new Cart()));
                }

                // Set the Rate property of the userCart object to the latest tax rate
                rate = await _rateRepository.GetLatestTaxRateAsync();

                cartItems = userCart.CartItems;
            }
            else
            {
                cartItems = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            }

            return View(new CartViewModel(new Cart { CartItems = cartItems, Rate = rate }));
        }




  
    public async Task<IActionResult> AddToCart(long id, int quantity = 1)
    {

        Product product = await _productService.GetProductByIdAsync(id);
        
        if (product == null)
        {
            return NotFound();
        }

        List<CartItem> cart;

        if (User.Identity.IsAuthenticated)
        {
            AppUser user = await _userManager.GetUserAsync(User);
            Cart userCart = await _context.Carts.Include(c => c.CartItems)
                                     .ThenInclude(ci => ci.Product)
                                     .Include(c => c.Rate)
                                     .FirstOrDefaultAsync(c => c.UserId == user.Id);


            TaxRate latestRate = await _rateRepository.GetLatestTaxRateAsync();

            if (userCart == null)
            {
                userCart = new Cart
                {
                    UserId = user.Id,
                    CartItems = new List<CartItem>(),
                    Rate = latestRate
                   
                };

                await _context.Carts.AddAsync(userCart);
            }

            CartItem cartItem = null;

            if(userCart != null){
                cartItem = userCart.CartItems.FirstOrDefault(ci => ci.Product.Id == id);
            }
           

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    Product = product,
                    Quantity = quantity,
                    CartId = userCart.Id
                };
                userCart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
        }
        else
        {
            cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            
            CartItem cartItem = null;

            if(cart != null){
                cartItem = cart.FirstOrDefault(ci => ci.Product != null && ci.Product.Id == id);

            }

            if (cartItem == null)
            {
                cart.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            HttpContext.Session.SetJson("Cart", cart);
        }

        return Redirect(Request.Headers["Referer"].ToString());
    }


        public async Task<IActionResult> Decrease(long id)
        {
            _cartService.DecreaseCartItemAsync((int)id);

            TempData["Success"] = "The Product has been Removed";
            return Redirect(Request.Headers["Referer"].ToString());
        }


        public async Task<IActionResult> Remove(long id)
        {
            _cartService.RemoveCartItemAsync((int) id);

            TempData["Error"] = "The product was not found in the cart.";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Clear()
        {
           
            _cartService.ClearCartAsync();
            TempData["Success"] = "The cart has been cleared!";
            return RedirectToAction("Index");
        }        

     }
}



