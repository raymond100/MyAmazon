
namespace ShoppingCart.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? UserId { get; set; } 
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsShipped { get; set; }
        public TaxRate Rate { get; set; }
        // other properties as needed

        public List<OrderItem> OrderItems { get; set; }

        public Order(){}

        public Order(string userId, string orderNumber, DateTime orderDate, decimal totalAmount, List<OrderItem> orderItems, TaxRate rate)
        {
            UserId = userId;
            OrderNumber = orderNumber;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            IsShipped = true;
            OrderItems = orderItems;
            Rate = rate;
        }

    }
}