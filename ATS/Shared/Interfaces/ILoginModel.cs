using Shared.Enums;

namespace Shared.Interfaces
{
    public interface ILoginModel
    {
        string? Email { get; set; }
        string? Password { get; set; }
        public UserRole Role { get; set; }
    }
}
