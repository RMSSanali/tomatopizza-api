using TomatoPizza.Data.Entities;

namespace TomatoPizza.Data.Repos
{
    public interface IIngredientRepo
    {
        Task<IEnumerable<Ingredient>> GetAllAsync();
        Task<Ingredient?> GetByNameAsync(string name);
        Task<Ingredient> CreateAsync(Ingredient ingredient);
    }
}
