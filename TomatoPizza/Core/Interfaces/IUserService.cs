using TomatoPizza.Data.DTO;
using TomatoPizza.Data.DTO.Users;

namespace TomatoPizza.Core.Interfaces
{
    public interface IUserService
    {
        Task<bool> Register(UserRegisterDTO user);
        Task<string?> Login(UserLoginDTO user); 
        Task<UserReadDTO?> GetProfile(string username);
        Task<bool> UpdateProfile(string username, UserUpdateDTO updatedData);
    }
}
