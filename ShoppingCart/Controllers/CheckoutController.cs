using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Infrastructure;

namespace ShoppingCart.Controllers
{

    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}