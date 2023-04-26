namespace ShoppingCart.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual List<CartItem> CartItems { get; set; }
        public TaxRate Rate { get; set; }


        public Cart(){

            CartItems = new List<CartItem>();
        }

        public decimal Total
        {
            get { return CartItems.Sum(c => c.Subtotal); }
        }


    }

}