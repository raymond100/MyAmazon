using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Services;
using Microsoft.AspNetCore.Identity;
using ShoppingCart.Models;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingCart.Controllers
{
    [Authorize(AuthenticationSchemes = "AuthScheme", Policy = "VendorPolicy")]
    public class VendorController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderItemService _orderItemService;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;

        public VendorController(IOrderItemService orderItemService,IProductService productService, IUserService userService,UserManager<AppUser> userManager){
            _productService = productService;
            _userService = userService;
            _userManager = userManager;
            _orderItemService = orderItemService;
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
                    ViewBag.OrderItems= await _orderItemService.GetOrderItemByVendorItAsync(vendorId);
                    var total = (decimal)0;
                    foreach( var data in ViewBag.OrderItems) {
                        total = total + data.Total;
                    }
                    var oderItemGroupByCategory = await _orderItemService.GetOrderItemByVendorItAsyncGrouByCategory(vendorId);
                    Dictionary<string, decimal> map = new Dictionary<string, decimal>();
                    foreach(var data in oderItemGroupByCategory){
                        var totalByCategory = (decimal)0;
                        foreach(var row in data){
                            totalByCategory = totalByCategory + row.Total;
                        }
                        map.Add(data.Key.Name, totalByCategory);
                    }
                    var dataOfCategory = map.Select(x => new {
                        Category = x.Key,
                        value = x.Value
                    }).ToList();
                    ViewBag.OderItemGroupByCategory = dataOfCategory;
                    ViewBag.TotalSales = total;
                }
            }
            return View();
        }



    }
}