using Shared.Enums;

namespace ATS.Api.Models
{
    public class ApiRegisterModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserRole Role { get; set; }
    }
}