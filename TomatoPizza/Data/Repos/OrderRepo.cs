using Microsoft.EntityFrameworkCore;
using TomatoPizza.Data.Entities;

namespace TomatoPizza.Data.Repos
{
    public class OrderRepo : IOrderRepo
    {
        private readonly AppDbContext _context;

        public OrderRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetOrdersByUserAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.Dishes)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Dishes)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
