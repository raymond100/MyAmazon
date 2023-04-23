
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Services;

namespace ShoppingCart.Controllers
{
    public class AdminController : Controller
    {

        private readonly IProductService _productService;

        public AdminController(IProductService productService){
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
           
            ViewBag.products = products;

            return View();
        }
    }
}