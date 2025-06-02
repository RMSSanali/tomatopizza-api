using System.ComponentModel.DataAnnotations;

namespace TomatoPizza.Data.DTO.Users
{
    public class UserUpdateDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [Phone]
        public string Phone { get; set; } = null!;
    }
}
