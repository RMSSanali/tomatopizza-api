using Microsoft.EntityFrameworkCore;
using TomatoPizza.Data.Entities;

namespace TomatoPizza.Data.Repos
{
    public class DishRepo : IDishRepo
    {
        private readonly AppDbContext _context;

        public DishRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Dish>> GetAllDishesAsync()
        {
            return await _context.Dishes
                .Include(d => d.Ingredients)
                .ToListAsync();
        }

        public async Task<Dish?> GetDishByIdAsync(int id)
        {
            return await _context.Dishes
                .Include(d => d.Ingredients)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddDishAsync(Dish dish)
        {
            await _context.Dishes.AddAsync(dish);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDishAsync(Dish dish)
        {
            _context.Dishes.Update(dish);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDishAsync(Dish dish)
        {
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DishExistsAsync(int id)
        {
            return await _context.Dishes.AnyAsync(d => d.Id == id);
        }
    }
}
