using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Infrastructure.Components
{
  public class SmallCartViewComponent : ViewComponent
{
        private readonly Cart _cart;

        public SmallCartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            SmallCartViewModel smallCartVM;

            if (_cart == null || _cart.CartItems.Count == 0)
            {
                smallCartVM = null;
            }
            else
            {
                smallCartVM = new SmallCartViewModel()
                {
                    NumberOfItems = _cart.CartItems.Sum(x => x.Quantity),
                    TotalAmount = _cart.CartItems.Sum(x => x.Quantity * x.Product.Price),
                };
            }

            return View(smallCartVM);
        }
    }

}



