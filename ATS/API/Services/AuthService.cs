using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Data;
using Shared.Interfaces;
using Shared.DTOs;
using ATS.Api.Models;
using System.Security.Cryptography;

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

        public async Task<bool> RegisterAsync(ApiRegisterModel model)
        {
            var existingUser = await _dbContext.Users.AnyAsync(u => u.Email == model.Email);
            if (existingUser)
            {
                return false; 
            }

            var newUser = new ApiLoginModel
            {
                Email = model.Email,
                Password = HashPassword(model.Password!)
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<LoginResponseDto?> LoginAsync(ApiLoginModel model)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                return null;
            }

            var token = GenerateToken(user.Email!);

            return new LoginResponseDto
            {
                AccessToken = token,
                RefreshToken = "dummy-refresh-token",
                UserName = user.Email!
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
