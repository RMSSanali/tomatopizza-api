namespace TomatoPizza.Data.DTO.Users
{
    public class UserReadDTO
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public List<string> Roles { get; set; } = new();
        public int BonusPoints { get; set; }
    }
}
