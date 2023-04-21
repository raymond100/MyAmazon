using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;
using ShoppingCart.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {

        private readonly DataContext _context;
        public CartController(DataContext contex)
        {
            _context = contex;
        }
        public IActionResult Index(String SelectedOption)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new(cart); 

            List<SelectListItem> options = new List<SelectListItem>();


            for (int i = 1; i <= 10; i++)
            {
                options.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
            }

            cartVM.Options = options;
    
            return View(cartVM);
        }

        public async Task<IActionResult> Add(long id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (id == null || product == null)
            {
                return NotFound();
            }

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();

            if (cartItem == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity += 1;
            }
            HttpContext.Session.SetJson("Cart", cart);
            TempData["Success"] = "The product has been added!";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Decrease(long id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            CartItem cartItem=cart.Where(x=>x.ProductId==id).FirstOrDefault();
            if (cartItem.Quantity >1)
            {
                --cartItem.Quantity;                
            }
            else
            {
                cart.RemoveAll(x=>x.ProductId==id);
            }
            if(cart.Count==0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            TempData["Success"] = "The Product has been Removed";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Remove(long id)
        {
           List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            cart.RemoveAll(p => p.ProductId == id);
            if(cart.Count==0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            TempData["Success"] = "The Product has been Removed";
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }


        public IActionResult addQuantity(long id, String SelectedOption){
            Console.WriteLine("Id: " + id);
            Console.WriteLine("Option: " + SelectedOption);
            return RedirectToAction("Index");
        }
        

    }
}



