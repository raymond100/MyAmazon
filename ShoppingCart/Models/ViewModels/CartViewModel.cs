namespace ShoppingCart.Models.ViewModels
{
    public class CartViewModel
    {

        public List<CartItem>  CartItems { get; set; }

        public CartViewModel(List<CartItem> cartItems){
            CartItems = cartItems;
        }
        private decimal total;
    
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
