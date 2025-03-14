using Shared.Enums;

namespace Shared.DTOs
{
    public class UpdateUserDto
    {
        public string? Email { get; set; }
        public UserRole? Role { get; set; } 
    }
}
