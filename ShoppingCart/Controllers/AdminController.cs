
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Services;

namespace ShoppingCart.Controllers
{
    public class AdminController : Controller
    {

        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public AdminController(IProductService productService, IUserService userService){
            _productService = productService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();

            var users =  await _userService.GetAllNonApprovedUsers();
            ViewBag.products = products;
            ViewBag.users = users;

            return View();
        }

        public async Task<IActionResult> Approve(string UserId)
        {
           _userService.ApproveUser(UserId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Reject(string UserId)
        {
            _userService.DeleteUser(UserId);

            return RedirectToAction(nameof(Index));
        }
    }
}
