using Microsoft.EntityFrameworkCore;
using TomatoPizza.Core.Interfaces;
using TomatoPizza.Data.DTO.Dishes;
using TomatoPizza.Data.Entities;
using TomatoPizza.Data.Repos;

namespace TomatoPizza.Core.Services
{
    public class DishService : IDishService
    {
        private readonly IDishRepo _dishRepo;
        private readonly AppDbContext _context;

        public DishService(IDishRepo dishRepo, AppDbContext context)
        {
            _dishRepo = dishRepo;
            _context = context;
        }

        public async Task<List<DishReadDTO>> GetAllDishesAsync()
        {
            var dishes = await _dishRepo.GetAllDishesAsync();

            return dishes.Select(d => new DishReadDTO
            {
                Id = d.Id,
                Name = d.Name,
                Price = d.Price,
                Description = d.Description,
                Category = d.Category,
                Ingredients = d.Ingredients.Select(i => i.Name).ToList()
            }).ToList();
        }

        public async Task<DishReadDTO?> GetDishByIdAsync(int id)
        {
            var dish = await _dishRepo.GetDishByIdAsync(id);
            if (dish == null) return null;

            return new DishReadDTO
            {
                Id = dish.Id,
                Name = dish.Name,
                Price = dish.Price,
                Description = dish.Description,
                Category = dish.Category,
                Ingredients = dish.Ingredients.Select(i => i.Name).ToList()
            };
        }

        public async Task<bool> CreateDishAsync(DishCreateDTO dto)
        {
            var ingredients = await GetOrCreateIngredientsAsync(dto.Ingredients);

            var dish = new Dish
            {
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                Category = dto.Category,
                Ingredients = ingredients
            };

            await _dishRepo.AddDishAsync(dish);
            return true;
        }

        public async Task<bool> UpdateDishAsync(int id, DishUpdateDTO dto)
        {
            var dish = await _dishRepo.GetDishByIdAsync(id);
            if (dish == null) return false;

            dish.Name = dto.Name;
            dish.Price = dto.Price;
            dish.Description = dto.Description;
            dish.Category = dto.Category;
            dish.Ingredients = await GetOrCreateIngredientsAsync(dto.Ingredients);

            await _dishRepo.UpdateDishAsync(dish);
            return true;
        }

        public async Task<bool> DeleteDishAsync(int id)
        {
            var dish = await _dishRepo.GetDishByIdAsync(id);
            if (dish == null) return false;

            await _dishRepo.DeleteDishAsync(dish);
            return true;
        }

        private async Task<List<Ingredient>> GetOrCreateIngredientsAsync(List<string> names)
        {
            var ingredients = new List<Ingredient>();

            foreach (var name in names.Distinct())
            {
                var ingredient = await _context.Ingredients
                    .FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower());

                if (ingredient == null)
                {
                    ingredient = new Ingredient { Name = name };
                    await _context.Ingredients.AddAsync(ingredient);
                    await _context.SaveChangesAsync();
                }

                ingredients.Add(ingredient);
            }

            return ingredients;
        }
    }
}
