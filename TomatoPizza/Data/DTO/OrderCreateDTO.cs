using System.ComponentModel.DataAnnotations;

namespace TomatoPizza.Data.DTO.Orders
{
    public class OrderCreateDTO
    {
        [Required]
        public List<int> DishIds { get; set; } = new();

        public bool UseBonusPoints { get; set; } = false;
    }
}
