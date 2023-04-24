
using ShoppingCart.Data;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingCart.Controllers
{
    // [Authorize(AuthenticationSchemes = "AuthScheme", Policy = "AdminPolicy")]
    public class AdminController : Controller
    {

        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;

        public AdminController(UserManager<AppUser> userManager,DataContext context,IProductService productService, IUserService userService,ICategoryService categoryService)
        {
            _context = context;
            _productService = productService;
            _userService = userService;
            _userManager = userManager;
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

        public async Task<IActionResult> RejectProduct(long id)
        {
            Product product = await _context.Products.FindAsync(id);
            _context.Remove(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Product has been rejected successfuly";
            return RedirectToAction("index");
        }
        public async Task<IActionResult> ApproveProduct(long id)
        {
            Product product = _context.Products.Find(id);
           // product.VendorId = _userManager.GetUserId(User);

            product.IsApproved = true;
            _context.Update(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Product has been approved successfuly";
            return RedirectToAction("index");
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
