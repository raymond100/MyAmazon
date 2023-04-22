using ShoppingCart.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart.Repository
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(long id);
        Task<List<Order>> GetAllOrdersAsync();
        Task<List<Order>> GetAllOrdersByUserIdAsync(string userId);
        Task<Order> AddOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(Order order);
        Task DeleteOrderByIdAsync(long id);
    }
}
