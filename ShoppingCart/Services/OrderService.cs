
using ShoppingCart.Repository;
using ShoppingCart.Models;

namespace  ShoppingCart.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetOrderByIdAsync(id);
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _orderRepository.AddOrderAsync(order);
            return order;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            await _orderRepository.UpdateOrderAsync(order);
            return order;
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteOrderByIdAsync(id);
        }
    }
}