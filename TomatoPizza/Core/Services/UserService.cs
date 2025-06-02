using Microsoft.AspNetCore.Identity;
using TomatoPizza.Core.Interfaces;
using TomatoPizza.Data.DTO.Users;
using TomatoPizza.Data.Identity;
using TomatoPizza.Security;

namespace TomatoPizza.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtService _jwtService;
        //private readonly IJwtService _jwtService;

        public UserService(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IJwtService jwtService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        // Register a new user and assign default role
        public async Task<bool> Register(UserRegisterDTO user)
        {
            var newUser = new AppUser
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.Phone
            };

            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "RegularUser");
            }

            return result.Succeeded;
        }

        // Login and return JWT token
        public async Task<string?> Login(UserLoginDTO user)
        {
            var existingUser = await _userManager.FindByNameAsync(user.UserName);
            if (existingUser == null)
                return null;

            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, false, false);

            if (!result.Succeeded)
                return null;

            var roles = await _userManager.GetRolesAsync(existingUser);
            return _jwtService.GenerateToken(existingUser, roles);
            //return _jwtService.GenerateToken(existingUser, roles);
        }

        // Get authenticated user's profile



        public async Task<UserReadDTO?> GetProfile(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            // Prioritize roles: Admin > PremiumUser > RegularUser
            string role = roles.Contains("Admin") ? "Admin"
                        : roles.Contains("PremiumUser") ? "PremiumUser"
                        : "RegularUser";

            return new UserReadDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Roles = roles.ToList(),
                BonusPoints = user.BonusPoints
            };
        }

        // Update authenticated user's email and phone
        public async Task<bool> UpdateProfile(string username, UserUpdateDTO updatedData)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return false;

            user.Email = updatedData.Email;
            user.PhoneNumber = updatedData.Phone;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
