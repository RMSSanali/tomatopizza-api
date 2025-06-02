using TomatoPizza.Data.DTO.Orders;

public class OrderReadDTO
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public decimal OriginalTotalPrice { get; set; }   // Price before discounts
    public decimal DiscountApplied { get; set; }       // Amount of discount
    public decimal TotalPrice { get; set; }            // Price after all discounts

    //public decimal FinalPrice => TotalPrice;           // Final price (redundant but OK for clarity/UI)

    public bool UsedBonus { get; set; }
    public string Status { get; set; } = null!;

    public List<OrderDishDTO> Dishes { get; set; } = new();

    public string? BonusInfo { get; set; }  // Message showing bonus points status
}
