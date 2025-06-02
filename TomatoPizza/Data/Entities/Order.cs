using TomatoPizza.Data.Entities;
using TomatoPizza.Data.Identity;

public class Order
{
    public int Id { get; set; }

    // Foreign Key to AppUser
    public string UserId { get; set; } = null!;

    public AppUser User { get; set; } = null!;  // Navigation property

    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Many-to-many with Dish
    public ICollection<Dish> Dishes { get; set; } = new List<Dish>();

    public string Status { get; set; } = "Pending";
    public bool UsedBonus { get; set; } = false;

}
