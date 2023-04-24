
namespace ShoppingCart.Models
{
    public class Order
    {

        public Order(){}
        public Order(string userId, string orderNumber, DateTime orderDate, decimal totalAmount, List<OrderItem> orderItems)
        {
            UserId = userId;
            OrderNumber = orderNumber;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            IsShipped = true;
            OrderItems = orderItems;
        }

        public int Id { get; set; }
        public string UserId { get; set; } 
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsShipped { get; set; }
        // other properties as needed

        public List<OrderItem> OrderItems { get; set; }


    }
}