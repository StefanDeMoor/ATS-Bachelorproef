using ATS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Data;

namespace API.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _config;

        public AuthService(ApplicationDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        public async Task<string> RegisterAsync(RegisterModel model)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == model.Email))
            {
                return "User already exists";
            }

 
            var hashedPassword = model.Password;

            var newUser = new LoginModel
            {
                Email = model.Email,
                Password = hashedPassword
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return "User registered successfully";
        }

        public async Task<LoginResponse?> LoginAsync(LoginModel model)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                return null; 
            }

            var token = GenerateToken(user.Email!);

            return new LoginResponse
            {
                AccessToken = token,
                RefreshToken = "dummy-refresh-token", 
                UserName = user.Email
            };
        }

        private string GenerateToken(string email)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("role", "user")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
