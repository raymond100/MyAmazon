
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingCart.Models;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Models.ViewModels
{
    public class CartViewModel
    {
        public Cart Cart { get; set; }

        public CartViewModel(Cart cart)
        {
            Cart = cart;

        }
    }


}
