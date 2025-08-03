using APIVault.API.DTOs.ApiKey;

namespace APIVault.API.Services.Interfaces
{
    public interface IApiKeyService
    {
        Task<ApiKeyResponseDto> GenerateApiKeyAsync(Guid userId);
        Task<List<ApiKeyResponseDto>> GetUserApiKeysAsync(Guid userId);
        Task<bool> RevokeApiKeyAsync(Guid apiKeyId, Guid userId);
        Task<List<ApiScopeDto>> GetUserApiScopesAsync(Guid userId);
    }
}
