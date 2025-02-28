using ATS.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ATS.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var message = await _authService.RegisterAsync(model);
            return Ok(new { message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var response = await _authService.LoginAsync(model);
            if (response != null)
            {
                return Ok(response);
            }
            return Unauthorized(new { message = "Invalid credentials" });
        }
    }
}
