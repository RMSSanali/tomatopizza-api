using TomatoPizza.Data.DTO.Dishes;

namespace TomatoPizza.Core.Interfaces
{
    public interface IDishService
    {
        Task<List<DishReadDTO>> GetAllDishesAsync();
        Task<DishReadDTO?> GetDishByIdAsync(int id);
        Task<bool> CreateDishAsync(DishCreateDTO dto);
        Task<bool> UpdateDishAsync(int id, DishUpdateDTO dto);
        Task<bool> DeleteDishAsync(int id);
    }
}
