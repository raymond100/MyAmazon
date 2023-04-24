
using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public interface IOrderItemService
    {
        Task<OrderItem> CreateOrderItemAsync(OrderItem orderItem);
        Task<List<OrderItem>> GetOrderItemByVendorItAsync(string vendorId);
    }
}