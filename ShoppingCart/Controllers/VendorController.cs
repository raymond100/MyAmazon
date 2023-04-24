using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Services;
using Microsoft.AspNetCore.Identity;
using ShoppingCart.Models;
using System.Security.Claims;


namespace ShoppingCart.Controllers
{
    public class VendorController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;

        public VendorController(IProductService productService, IUserService userService,UserManager<AppUser> userManager){
            _productService = productService;
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            if(user != null)
            {
                if (await _userManager.IsInRoleAsync(user, "Vendor"))
                {
                    var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (!string.IsNullOrEmpty(vendorId))
                    {
                        var products = await _productService.GetAllProductsAsyncByVendorId(vendorId);
                        ViewBag.products = products;
                    }
                    else
                    {
                        // Handle the case where the user does not have a VendorId claim
                    }
                }
                else
                {
                    // Handle the case where the user is not a vendor
                }
            }

          
            // ViewBag.users = users;

            return View();
        }



    }
}