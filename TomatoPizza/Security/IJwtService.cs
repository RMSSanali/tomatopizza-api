using TomatoPizza.Data.Identity;

namespace TomatoPizza.Security
{
    public interface IJwtService
    {
        //string Generate (string username, string[] roles);
        string? GenerateToken(AppUser existingUser, IList<string> roles);
    }
}
