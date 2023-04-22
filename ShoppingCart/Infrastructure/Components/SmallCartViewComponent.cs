using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;
using ShoppingCart.Services;
using ShoppingCart.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ShoppingCart.Infrastructure.Components
{
  public class SmallCartViewComponent : ViewComponent
{
      

        private readonly ICartService _cartService;
      
       
        public SmallCartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            SmallCartViewModel smallCartVM;

            List<CartItem> cartItems = await _cartService.GetCartItemsAsync();

            smallCartVM = new SmallCartViewModel()
            {
                NumberOfItems = cartItems.Sum(x => x.Quantity),
                TotalAmount = cartItems.Sum(x => x.Quantity * x.Product.Price),
            };

            return View(smallCartVM);
        }

    }

}



