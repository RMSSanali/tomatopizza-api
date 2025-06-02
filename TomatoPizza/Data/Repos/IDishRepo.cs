using TomatoPizza.Data.Entities;

namespace TomatoPizza.Data.Repos
{
    public interface IDishRepo
    {
        Task<List<Dish>> GetAllDishesAsync();
        Task<Dish?> GetDishByIdAsync(int id);
        Task AddDishAsync(Dish dish);
        Task UpdateDishAsync(Dish dish);
        Task DeleteDishAsync(Dish dish);
        Task<bool> DishExistsAsync(int id);
    }
}
