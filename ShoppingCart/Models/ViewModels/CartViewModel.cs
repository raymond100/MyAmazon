
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoppingCart.Models.ViewModels
{
    public class CartViewModel
    {

        public List<CartItem>  CartItems { get; set; }

        public string SelectedOption { get; set; }

        public List<SelectListItem> Options { get; set; }

        public CartViewModel(List<CartItem> cartItems){
            CartItems = cartItems;
        }
        private decimal total;

        private int NumberOfItemsCount;

        public int NumberOfItems
        {
            get
            {
              return CartItems.Sum(x => x.Quantity);  
            }

            set
            {
                NumberOfItemsCount = NumberOfItems;
            }
        }
    
        public decimal GrandTotal
        { 
            get 
            {
                return CartItems.Sum(x => x.Quantity * x.Price);
            }

            set
            {
                total = GrandTotal;
            }
            

         }

    }
}
