using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;
using ShoppingCart.Services;
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

        public CartController(DataContext context, UserManager<AppUser> userManager, IProductService productService)
        {
            _context = context;
            _userManager = userManager;
            _productService = productService;
        }

    public async Task<IActionResult> Index()
{
    List<CartItem> cartItems;

    if (User.Identity.IsAuthenticated)
    {
        AppUser user = await _userManager.GetUserAsync(User);
        Cart userCart = await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product)
                                .FirstOrDefaultAsync(c => c.UserId == user.Id);

        if (userCart == null)
        {
            return View(new CartViewModel(new Cart()));
        }

        cartItems = userCart.CartItems;
    }
    else
    {
        cartItems = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
    }

    return View(new CartViewModel(new Cart { CartItems = cartItems }));
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
            Cart userCart = await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product)
                                    .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (userCart == null)
            {
                userCart = new Cart
                {
                    UserId = user.Id,
                    CartItems = new List<CartItem>()
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




    //     public async Task<IActionResult> Add(long id)
    //     {
    //         Product product = await _context.Products.FindAsync(id);
    //         if (product == null)
    //         {
    //             return NotFound();
    //         }

    //         List<CartItem> cart;

    //         if (User.Identity.IsAuthenticated)
    //         {
    //             AppUser user = await _userManager.GetUserAsync(User);
    //             cart = _context.CartItems.Where(c => c.UserId == user.Id).ToList();

    //             CartItem existingCartItem = cart.Where(c => c.id == id).FirstOrDefault();

    //             if (existingCartItem == null)
    //             {
    //                 _context.CartItems.Add(new CartItem(product, user.Id));
    //             }
    //             else
    //             {
    //                 existingCartItem.Quantity += 1;
    //             }

    //             await _context.SaveChangesAsync();
    //         }
    //         else
    //         {
    //             cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

    //             CartItem cartItem = cart.Where(c => c.id == id).FirstOrDefault();

    //             if (cartItem == null)
    //             {
    //                 cart.Add(new CartItem(product));
    //             }
    //             else
    //             {
    //                 cartItem.Quantity += 1;
    //             }

    //             HttpContext.Session.SetJson("Cart", cart);
    //         }

    //         TempData["Success"] = "The product has been added!";
    //         return Redirect(Request.Headers["Referer"].ToString());
    //     }

    //     public async Task<IActionResult> Decrease(long id)
    //     {
    //         List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
    //         CartItem cartItem = cart.Where(x => x.id == id).FirstOrDefault();

    //         if (cartItem.Quantity > 1)
    //         {
    //             --cartItem.Quantity;
    //         }
    //         else
    //         {
    //             cart.RemoveAll(x => x.id == id);
    //         }

    //         if (cart.Count == 0)
    //         {
    //             HttpContext.Session.Remove("Cart");
    //         }
    //         else
    //         {
    //             HttpContext.Session.SetJson("Cart", cart);
    //         }

    //         if (User.Identity.IsAuthenticated)
    //         {
    //             AppUser user = await _userManager.GetUserAsync(User);
    //             CartItem existingCartItem = _context.CartItems.Where(c => c.id == id && c.UserId == user.Id).FirstOrDefault();

    //             if (existingCartItem != null)
    //             {
    //                 if (existingCartItem.Quantity > 1)
    //                 {
    //                     --existingCartItem.Quantity;
    //                 }
    //                 else
    //                 {
    //                     _context.CartItems.Remove(existingCartItem);
    //                 }

    //                 await _context.SaveChangesAsync();
    //             }
    //         }

    //         TempData["Success"] = "The Product has been Removed";
    //         return Redirect(Request.Headers["Referer"].ToString());
    //     }


    //     public async Task<IActionResult> Remove(long id)
    //     {
    //         List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
    //         CartItem cartItem = cart.Where(x => x.id == id).FirstOrDefault();

    //         if (cartItem != null)
    //         {
    //             cart.RemoveAll(x => x.id == id);

    //             if (cart.Count == 0)
    //             {
    //                 HttpContext.Session.Remove("Cart");
    //             }
    //             else
    //             {
    //                 HttpContext.Session.SetJson("Cart", cart);
    //             }

    //             if (User.Identity.IsAuthenticated)
    //             {
    //                 AppUser user = await _userManager.GetUserAsync(User);
    //                 CartItem existingCartItem = _context.CartItems.Where(c => c.id == id && c.UserId == user.Id).FirstOrDefault();

    //                 if (existingCartItem != null)
    //                 {
    //                     _context.CartItems.Remove(existingCartItem);
    //                     await _context.SaveChangesAsync();
    //                 }
    //             }

    //             TempData["Success"] = "The product has been removed from the cart!";
    //         }
    //         else
    //         {
    //             TempData["Error"] = "The product was not found in the cart.";
    //         }

    //         return Redirect(Request.Headers["Referer"].ToString());
    //     }

    //     public async Task<IActionResult> Clear()
    //     {
    //         HttpContext.Session.Remove("Cart");

    //         if (User.Identity.IsAuthenticated)
    //         {
    //             AppUser user = await _userManager.GetUserAsync(User);
    //             _context.CartItems.RemoveRange(_context.CartItems.Where(c => c.UserId == user.Id));
    //             await _context.SaveChangesAsync();
    //         }

    //         TempData["Success"] = "The cart has been cleared!";
    //         return RedirectToAction("Index");
    //     }        

     }
}



