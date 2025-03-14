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
                Password = HashPassword(model.Password!),
                Role = model.Role
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

            if (!VerifyPassword(model.Password!, user.Password!))
            {
                return null;
            }

            var token = GenerateJwtToken();

            return new LoginResponseDto
            {
                AccessToken = token,
                RefreshToken = "dummy-refresh-token",
                UserName = user.Email!
            };
        }

        private bool VerifyPassword(string providedPassword, string storedPasswordHash)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(providedPassword);
            var hash = sha256.ComputeHash(bytes);
            var computedHash = Convert.ToBase64String(hash);

            return computedHash == storedPasswordHash;
        }
        public string GenerateJwtToken()
        {
            var secretKey = _config["Jwt:Key"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var issuedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var expiration = issuedAt + 1800; // Token expires in 30 minutes

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, "root"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, issuedAt.ToString(), ClaimValueTypes.Integer64),
        new Claim("user_name", "root"),
        new Claim("user_id", "13dab5e3-deba-46a1-8d03-08d731ce02d6"),
        new Claim("language", "en"),
        new Claim("timezone", "Europe/Brussels"),
        new Claim("loggedInWithAD", "False"),
        new Claim("P_Dashboards", "f"),
        new Claim("P_DataSources", "f"),
        new Claim("P_Settings", "f"),
        new Claim("P_Task", "f"),
        new Claim("refresh_token", Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Nbf, issuedAt.ToString(), ClaimValueTypes.Integer64),
        new Claim(JwtRegisteredClaimNames.Exp, expiration.ToString(), ClaimValueTypes.Integer64),
        new Claim(JwtRegisteredClaimNames.Iss, _config["Jwt:Issuer"]!),
        new Claim(JwtRegisteredClaimNames.Aud, _config["Jwt:Audience"]!)
    };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTimeOffset.FromUnixTimeSeconds(expiration).UtcDateTime,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
