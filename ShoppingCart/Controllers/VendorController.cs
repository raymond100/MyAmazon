
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Services;

namespace ShoppingCart.Controllers
{
    public class VendorController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public VendorController(IProductService productService, IUserService userService){
            _productService = productService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            // var vendorId = await _userManager.GetUserIdAsync(HttpContext.User);
            // var products = await _productService.GetAllProductsAsyncByVendorId(vendorId);
            
            // // var users =  await _userService.GetAllNonApprovedUsers();
            // ViewBag.products = products;
            // ViewBag.users = users;

            return View();
        }



    }
}