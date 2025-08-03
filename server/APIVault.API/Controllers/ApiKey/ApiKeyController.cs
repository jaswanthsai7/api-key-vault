using APIVault.API.DTOs.ApiKey;
using APIVault.API.DTOs.Common;
using APIVault.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIVault.API.Controllers.ApiKey
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

        // Generate API Key
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateApiKey()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var apiKey = await _apiKeyService.GenerateApiKeyAsync(userId);
                return Ok(new { apiKey });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("Error generating API key.", ex.Message));
            }
        }

        // View API Keys
        [HttpGet("my")]
        public async Task<IActionResult> GetMyApiKeys()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var keys = await _apiKeyService.GetUserApiKeysAsync(userId);
                return Ok(keys);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("Error fetching API keys.", ex.Message));
            }
        }

        // Revoke API Key
        [HttpPut("revoke/{apiKeyId}")]
        public async Task<IActionResult> RevokeApiKey(Guid apiKeyId)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var success = await _apiKeyService.RevokeApiKeyAsync(apiKeyId, userId);
                if (!success)
                    return NotFound(new ErrorResponse("API key not found or unauthorized."));
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("Error revoking API key.", ex.Message));
            }
        }

        // View API Scopes
        [HttpGet("scopes")]
        public async Task<IActionResult> GetMyApiScopes()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var scopes = await _apiKeyService.GetUserApiScopesAsync(userId);
                return Ok(scopes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("Error fetching scopes.", ex.Message));
            }
        }

        // Extract user ID from JWT
        private Guid GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token.");
            return Guid.Parse(userIdClaim.Value);
        }
    }
}
