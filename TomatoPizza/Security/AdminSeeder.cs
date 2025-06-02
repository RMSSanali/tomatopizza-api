using Microsoft.AspNetCore.Identity;
using TomatoPizza.Data.Identity;

namespace TomatoPizza.Security
{
    public static class AdminSeeder
    {
        public static async Task SeedAdmin(UserManager<AppUser> userManager)
        {
            string adminUserName = "admin";
            string adminEmail = "admin@pizza.com";
            string adminPassword = "Admin@123";

            var existingAdmin = await userManager.FindByNameAsync(adminUserName);
            if (existingAdmin == null)
            {
                var admin = new AppUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
