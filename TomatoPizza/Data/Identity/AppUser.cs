using Microsoft.AspNetCore.Identity;

namespace TomatoPizza.Data.Identity
{
    public class AppUser : IdentityUser
    {
        public int BonusPoints { get; set; } = 0;
    }    
}
