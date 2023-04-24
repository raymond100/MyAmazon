
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Services;

namespace ShoppingCart.Controllers
{
    public class AdminController : Controller
    {

        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;

        public AdminController(IProductService productService, IUserService userService, ICategoryService categoryService)
        {
            _productService = productService;
            _userService = userService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();

            var users =  await _userService.GetAllNonApprovedUsers();

            var category = await _categoryService.GetAllAsync();

            ViewBag.products = products;
            ViewBag.users = users;
            ViewBag.category = category;

            return View();
        }
    }
}