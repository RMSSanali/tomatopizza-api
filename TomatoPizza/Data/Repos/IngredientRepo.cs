using Microsoft.EntityFrameworkCore;
using TomatoPizza.Data.Entities;

namespace TomatoPizza.Data.Repos
{
    public class IngredientRepo : IIngredientRepo
    {
        private readonly AppDbContext _context;

        public IngredientRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ingredient>> GetAllAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }

        public async Task<Ingredient?> GetByNameAsync(string name)
        {
            return await _context.Ingredients
                .FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower());
        }

        public async Task<Ingredient> CreateAsync(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return ingredient;
        }
    }
}
