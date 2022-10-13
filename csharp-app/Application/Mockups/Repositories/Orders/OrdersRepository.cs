using Mockups.Storage;
using Microsoft.EntityFrameworkCore;

namespace Mockups.Repositories.Orders
{
    public class OrdersRepository
    {
        private readonly ApplicationDbContext _context;

        public OrdersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetUsersPastOrders(Guid userId)
        {
            return await _context.Orders
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderById(int orderId)
        {
            return await _context.Orders.Where(x => x.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task EditOrder(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders
                .ToListAsync();
        }

        public async Task<List<OrderItem>> GetOrdersItems(int orderId)
        {
            return await _context.OrderMenuItems
                .Where(x => x.OrderId == orderId)
                .Select(oi => new OrderItem
                {
                    Item = oi.Item,
                    Amount = oi.Amount
                })
                .ToListAsync();
        }

        public async Task<int> AddOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task AddOrderMenuItem(OrderMenuItem item)
        {
            await _context.OrderMenuItems.AddAsync(item);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}