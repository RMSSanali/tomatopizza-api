namespace TomatoPizza.Data.DTO.Dishes
{
    public class DishUpdateDTO
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;

        public List<string> Ingredients { get; set; } = new();
    }
}
