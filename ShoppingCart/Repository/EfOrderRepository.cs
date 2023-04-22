using ShoppingCart.Data;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;

namespace ShoppingCart.Repository
{
    public class EfOrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public EfOrderRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId.ToString() == userId)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(long id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task DeleteOrderAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderByIdAsync(long id)
        {
            var order = await GetOrderByIdAsync(id);
            if (order != null)
            {
                await DeleteOrderAsync(order);
            }
        }
    }
}