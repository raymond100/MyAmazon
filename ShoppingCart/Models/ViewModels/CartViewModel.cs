
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


        public decimal TaxAmount()
        {
            decimal taxAmount = 0;
            foreach (var item in Cart.CartItems)
            {
                decimal rate = Cart.Rate?.Rate ?? 0.17M; // if Rate is null, set rate to zero
                taxAmount += item.Price * item.Quantity * rate;
            }

            return taxAmount;
        }

        public decimal TotalAmount(){
            return Cart.Total + TaxAmount();
        }

    }

}
