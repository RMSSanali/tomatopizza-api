namespace TomatoPizza.Data.DTO.Dishes
{
    public class DishCreateDTO
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;

        // Ingredient names to link (admin enters ingredient strings)
        public List<string> Ingredients { get; set; } = new();
    }
}
