using APIVault.API.DTOs.Common;
using APIVault.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIVault.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApiKeyController : ControllerBase
    {
        private readonly IApiKeyService _apiKeyService;

        public ApiKeyController(IApiKeyService apiKeyService)
        {
            _apiKeyService = apiKeyService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateApiKey()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized(new ErrorResponse("User not authenticated."));

                var userId = Guid.Parse(userIdClaim.Value);

                var apiKey = await _apiKeyService.GenerateApiKeyAsync(userId);

                return Ok(new { apiKey });
            }
            catch (FormatException)
            {
                return BadRequest(new ErrorResponse("Invalid User ID format in token."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("An error occurred while generating the API key.", ex.Message));
            }
        }
    }
}
