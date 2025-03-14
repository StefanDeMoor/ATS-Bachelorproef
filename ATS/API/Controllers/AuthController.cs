using ATS.Api.Models;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using Shared.DTOs;

namespace Api.Controllers
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
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var registerModel = new ApiRegisterModel
            {
                Email = request.Email,
                Password = request.Password
            };

            var isRegistered = await _authService.RegisterAsync(registerModel);

            if (isRegistered)
            {
                return Ok(new { message = "User registered successfully" });
            }

            return BadRequest(new { message = "Email is already in use" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var loginModel = new ApiLoginModel
            {
                Email = request.Email,
                Password = request.Password
            };

            var response = await _authService.LoginAsync(loginModel);

            if (response != null)
            {
                return Ok(response);
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var isDeleted = await _authService.DeleteUserAsync(id);
            if (!isDeleted)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(new { message = "User deleted successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto request)
        {
            var isUpdated = await _authService.UpdateUserAsync(id, request);
            if (!isUpdated)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(new { message = "User updated successfully" });
        }

    }
}
