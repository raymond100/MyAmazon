namespace ShoppingCart.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        //public int ProductId { get; set; }
        public int Quantity { get; set; }
        //public string UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public virtual Product Product { get; set; }
        public virtual AppUser User { get; set; } 
        public int CartId { get; set; } 
        
        // Additional properties for display in the view
        public string Image { get { return Product?.Image ?? ""; } }
        public string ProductName { get { return Product?.Name ?? ""; } }
        public decimal Price { get { return Product?.Price ?? 0; } }

    public CartItem()
    {
       
    }

     public CartItem(Product product)
    {
        Product = product;
        Quantity = 1; // set default quantity to 1
    }

    // public CartItem(Product product, String userId)
    // {
    //     Product = product;
    //     UserId = userId;
    // }

    public CartItem(Product product, int quantity = 1)
    {
        Product = product;
        Quantity = quantity;
    }

    public CartItem(Product product, int cartId, int quantity = 1)
    {
        Product = product;
        Quantity = quantity;
        CartId = cartId;
    }

    public decimal Subtotal
    {
        get { return (Product != null && Quantity > 0) ? Quantity * Product.Price : 0; }
    }


    }
}
