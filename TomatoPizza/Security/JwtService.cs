using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TomatoPizza.Data.Identity;

namespace TomatoPizza.Security
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(AppUser user, IList<string> roles)
        {
            //var claims = new List<Claim>
            //{
            //new Claim(ClaimTypes.Name, user.UserName!),
            ////new Claim(JwtRegisteredClaimNames.Sub, user.Id), // ✅ this is your fix
            //new Claim(ClaimTypes.NameIdentifier, user.Id),
            //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            //};

            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, user.UserName!),              // for User.Identity.Name
            new Claim(ClaimTypes.NameIdentifier, user.Id),           // ✅ required for identifying the user ID
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),         // ✅ standard practice, also helps in APIs
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // unique token ID
            };


            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpireMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
