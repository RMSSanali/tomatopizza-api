using TomatoPizza.Data.Entities;

namespace TomatoPizza.Data.Repos
{
    public interface IOrderRepo
    {
        Task<Order> CreateAsync(Order order);
        Task<List<Order>> GetOrdersByUserAsync(string userId);
        Task<Order?> GetByIdAsync(int id);
    }
}
