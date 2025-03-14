using Shared.Enums;

namespace Shared.DTOs
{
    public class RegisterRequestDto
    {
        //Dit is wat we moeten invullen in swagger
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserRole? Role { get; set; }
    }
}