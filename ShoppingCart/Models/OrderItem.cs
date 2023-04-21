namespace ShoppingCart.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public virtual Product Product { get; set; }
        // other properties as needed

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}