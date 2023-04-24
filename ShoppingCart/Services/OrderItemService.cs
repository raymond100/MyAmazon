
using ShoppingCart.Repository;
using ShoppingCart.Models;

namespace  ShoppingCart.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task<OrderItem> CreateOrderItemAsync(OrderItem orderItem)
        {
            await _orderItemRepository.AddOrderItemAsync(orderItem);
            return orderItem;
        }
        
        public async Task<List<OrderItem>> GetOrderItemByVendorItAsync(string vendorId){
            return await _orderItemRepository.GetOrderItemByVendorItAsync(vendorId);
        }
    }
}