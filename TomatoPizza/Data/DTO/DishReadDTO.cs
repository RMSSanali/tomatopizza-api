namespace TomatoPizza.Data.DTO.Dishes
{
    public class DishReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;

        // Return ingredient names
        public List<string> Ingredients { get; set; } = new();
    }
}
