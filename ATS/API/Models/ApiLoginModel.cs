using Shared.Interfaces;

namespace ATS.Api.Models
{
    public class ApiLoginModel : ILoginModel
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
