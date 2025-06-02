using TomatoPizza.Data.DTO.Ingredients;

namespace TomatoPizza.Core.Interfaces
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientReadDTO>> GetAllAsync();
    }
}
