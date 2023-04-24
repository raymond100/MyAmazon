using ShoppingCart.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart.Repository
{
    public interface IOrderItemRepository
    {
        Task<OrderItem> AddOrderItemAsync(OrderItem orderItem);
        Task<List<OrderItem>> GetOrderItemByVendorItAsync(string vendorId);
    }
}
