using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Controllers
{

 public class ReportController : Controller{
    public IActionResult Index()
    {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new(cart);
        
            ReportViewModel reportVm = new(cartVM);
             
            //ViewData["data"] = reportVm;

            return View(reportVm);
    }
 }
    
}