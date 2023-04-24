namespace ShoppingCart.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public virtual Product Product { get; set; }
        public DateTime OrderDate { get; set; }
        // other properties as needed

       // public virtual Order Order { get; set; }

        public decimal Total
        {
            get { return Quantity * Price; }
        }

        public OrderItem(){}

        public OrderItem(Product product, string productName, int quantity, decimal price)
        {
            Product = product;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }
    }

}