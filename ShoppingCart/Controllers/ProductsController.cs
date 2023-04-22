using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
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
        private readonly DataContext dataContext;

        public ProductsController(IProductService productService, DataContext _dataContext)
        {
            _productService = productService;
            dataContext = _dataContext;
        }

        public async Task<IActionResult> Index(string searchString, string categorySlug = "", int p = 1)
        {
            var products = await _productService.GetAllProductsAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.ToLower().Contains(searchString.ToLower())).ToList();
                return View(products);
            }

            int pageSize = 3;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.CategorySlug = categorySlug;

            if (categorySlug == "")
            {
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)products.Count() / pageSize);

               // return View(products.OrderByDescending(p => p.Id).Skip((p - 1) * pageSize).Take(pageSize).ToList());
                return View(await dataContext.Products.OrderByDescending(p => p.Id).Skip((p - 1) * pageSize).Take(pageSize).ToListAsync());

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
