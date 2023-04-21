
namespace ShoppingCart.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsShipped { get; set; }
        // other properties as needed

        public List<OrderItem> OrderItems { get; set; }
    }
}