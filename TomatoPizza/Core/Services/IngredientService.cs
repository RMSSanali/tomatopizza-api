using TomatoPizza.Core.Interfaces;
using TomatoPizza.Data.DTO.Ingredients;
using TomatoPizza.Data.Repos;

namespace TomatoPizza.Core.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepo _ingredientRepo;

        public IngredientService(IIngredientRepo ingredientRepo)
        {
            _ingredientRepo = ingredientRepo;
        }

        public async Task<IEnumerable<IngredientReadDTO>> GetAllAsync()
        {
            var ingredients = await _ingredientRepo.GetAllAsync();

            return ingredients.Select(i => new IngredientReadDTO
            {
                Id = i.Id,
                Name = i.Name
            });
        }
    }
}
