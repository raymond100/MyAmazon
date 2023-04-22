using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;
using ShoppingCart.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(string searchString, string categorySlug = "", int p = 1)
        {
            //  var products = await _productService.GetAllProductsAsync();  changed

            var products = await _productService.GetAllApprovedProductsAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.ToLower().Contains(searchString.ToLower())).ToList();
                return View(products);
            }

            int pageSize = 10;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.CategorySlug = categorySlug;

            if (categorySlug == "")
            {
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)products.Count() / pageSize);

                return View(products.OrderByDescending(p => p.Id).Skip((p - 1) * pageSize).Take(pageSize).ToList());
            }

            Category category = await _productService.GetCategoryBySlugAsync(categorySlug);

            if (category == null)
            {
                return RedirectToAction("Index");
            }

            var productsByCategory = await _productService.GetProductsByCategoryIdAsync((int)category.Id);
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)productsByCategory.Count() / pageSize);

            return View(productsByCategory.OrderByDescending(p => p.Id).Skip((p - 1) * pageSize).Take(pageSize).ToList());
        }
    }
}
