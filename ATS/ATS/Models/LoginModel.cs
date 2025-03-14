using Shared.Enums;
using Shared.Interfaces;

namespace ATS.Models
{
    public class LoginModel : ILoginModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserRole Role { get; set; }
    }
}
