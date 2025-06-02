using System.ComponentModel.DataAnnotations;

namespace TomatoPizza.Data.Entities
{
    public class Dish
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; } = null!;

        [Required]
        public string Category { get; set; } = null!; // Pizza, Pasta, Salad, etc.

        // Many-to-many with ingredients
        public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        // Many-to-many with orders (used by EF Core to track which orders include this dish)
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
