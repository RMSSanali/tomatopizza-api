using System.ComponentModel.DataAnnotations;
using TomatoPizza.Data.Entities;

public class Ingredient
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    // Many-to-many with Dish
    public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
