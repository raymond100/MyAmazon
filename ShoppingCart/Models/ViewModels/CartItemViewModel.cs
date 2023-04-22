using ShoppingCart.Models;

namespace ShoppingCart.Models.ViewModels
{
    public class CartItemViewModel
    {
        //public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }

        public CartItemViewModel(CartItem item)
        {
            //ProductId = item.ProductId;
            Name = item.Product.Name;
            Description = item.Product.Description;
            Price = item.Product.Price;
            Quantity = item.Quantity;
            Image = item.Product.Image;
        }
    }


}