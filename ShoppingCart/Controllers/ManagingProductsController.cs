using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace ShoppingCart.Controllers
{
    //[Area("Vendor")]
    //[Authorize]
    public class ManagingProductsController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ManagingProductsController(DataContext context, IWebHostEnvironment webHostEnvironment,UserManager<AppUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 3;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);

            return View(await _context.Products.OrderByDescending(p => p.Id)
                                                .Include(p => p.Category)
                                                .Skip((p - 1) * pageSize)
                                                .Take(pageSize)
                                                .ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            product.VendorId = _userManager.GetUserId(User);
            if (product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;

                string filePath = Path.Combine(uploadsDir, imageName);

                FileStream fs = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close();

                product.Image = imageName;

            }
            _context.Add(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "The Product has been created!";
            return RedirectToAction("index", "vendor");
        }

        public  IActionResult Edit(long id)
        {
            Product product =  _context.Products.Find(id);
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Product product)
        {
             product.VendorId = _userManager.GetUserId(User);
            if (product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;

                string filePath = Path.Combine(uploadsDir, imageName);

                FileStream fs = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close();

                product.Image = imageName;

            }
            _context.Update(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "The Product has been edited!";
            return RedirectToAction("index", "vendor");
        }

        public async Task<IActionResult> Delete(long id)
        {
            Product? product = await _context.Products.FindAsync(id);

            if (!string.Equals(product.Image, "noimage.png"))
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                var oldpath = Path.Combine(uploadDir, product.Image);
                if (System.IO.File.Exists(oldpath))
                    System.IO.File.Delete(oldpath);

            }
            _context.Remove(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Product has been deleted successfuly";
            return RedirectToAction("index", "vendor");
        }
    }
}


