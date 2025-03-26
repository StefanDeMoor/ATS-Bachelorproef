using Shared.Enums;

namespace Shared.DTOs
{
    public class LoginRequestDto
    {
        //Dit is wat we moeten invullen in swagger
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
