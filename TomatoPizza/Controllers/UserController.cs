using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TomatoPizza.Core.Interfaces;
using TomatoPizza.Data.DTO.Users;
using TomatoPizza.Data.Identity;


namespace TomatoPizza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/User/login
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> UserLogin([FromBody] UserLoginDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _userService.Login(user);

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalid username or password.");

            return Ok(new { token });
        }

        // POST: api/User/register
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> UserRegister([FromBody] UserRegisterDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _userService.Register(user);

            if (!success)
                return BadRequest("Registration failed. User might already exist.");

            return Ok("User registered successfully.");
        }

        // GET: api/User/profile
        [Authorize]
        [HttpGet("ViewProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized("No authenticated user.");

            var userProfile = await _userService.GetProfile(username);
            if (userProfile == null)
                return NotFound("User not found.");

            return Ok(userProfile);
        }

        // PUT: api/User/profile
        [Authorize]
        [HttpPut("Update_profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDTO updatedData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized("No authenticated user.");

            var result = await _userService.UpdateProfile(username, updatedData);
            if (!result)
                return BadRequest("Update failed.");

            return Ok("Profile updated successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AdminPromote/{username}")]
        public async Task<IActionResult> PromoteToPremium(string username, [FromServices] UserManager<AppUser> userManager)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound("User not found.");

            var alreadyInRole = await userManager.IsInRoleAsync(user, "PremiumUser");
            if (alreadyInRole)
                return BadRequest("User is already a PremiumUser.");

            var result = await userManager.AddToRoleAsync(user, "PremiumUser");
            if (!result.Succeeded)
                return StatusCode(500, "Failed to promote user.");

            return Ok($"{username} has been promoted to PremiumUser.");
        }
    }
}
