using APIVault.API.DTOs.Auth;
using APIVault.API.Models;
using APIVault.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIVault.API.Controllers.Auth
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = await _authService.RegisterAsync(request);
                return Ok(new
                {
                    Message = "User registered successfully",
                    User = new
                    {
                        user.Id,
                        user.Email,
                        Role = user.RoleId,
                        Group = user.GroupId
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var (user, role, token) = await _authService.LoginAsync(request);

                Response.Cookies.Append("access_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // use HTTPS in production
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });

                return Ok(new
                {
                    message = "Login successful",
                    role = role
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(new
            {
                Id = userId,
                Email = email,
                Role = role
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return Ok(new { message = "Logged out successfully" });
        }

    }
}
