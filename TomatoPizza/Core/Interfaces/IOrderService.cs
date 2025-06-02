using TomatoPizza.Data.DTO.Orders;

namespace TomatoPizza.Core.Interfaces
{
    public interface IOrderService
    {
        Task<OrderReadDTO> CreateOrderAsync(string userId, OrderCreateDTO dto);
        Task<List<OrderReadDTO>> GetOrdersByUserAsync(string userId);
    }
}
