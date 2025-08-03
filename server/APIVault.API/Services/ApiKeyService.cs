using APIVault.API.Data;
using APIVault.API.DTOs.ApiKey;
using APIVault.API.Models;
using APIVault.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIVault.API.Services.Implementations
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly AppDbContext _context;

        public ApiKeyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiKeyResponseDto> GenerateApiKeyAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Group)
                .ThenInclude(g => g.GroupApiScopes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            var newKey = new ApiKey
            {
                Key = Guid.NewGuid().ToString("N"),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ApiKeyScopes = user.Group.GroupApiScopes
                    .Select(gas => new ApiKeyScope
                    {
                        ApiScopeId = gas.ApiScopeId
                    }).ToList()
            };

            _context.ApiKeys.Add(newKey);
            await _context.SaveChangesAsync();

            return new ApiKeyResponseDto
            {
                Id = newKey.Id,
                Key = newKey.Key,
                CreatedAt = newKey.CreatedAt,
                IsRevoked = newKey.IsRevoked
            };
        }

        public async Task<List<ApiKeyResponseDto>> GetUserApiKeysAsync(Guid userId)
        {
            return await _context.ApiKeys
                .Where(k => k.UserId == userId)
                .Select(k => new ApiKeyResponseDto
                {
                    Id = k.Id,
                    Key = k.Key,
                    CreatedAt = k.CreatedAt,
                    IsRevoked = k.IsRevoked
                }).ToListAsync();
        }

        public async Task<bool> RevokeApiKeyAsync(Guid apiKeyId, Guid userId)
        {
            var key = await _context.ApiKeys
                .FirstOrDefaultAsync(k => k.Id == apiKeyId && k.UserId == userId);

            if (key == null) return false;

            key.IsRevoked = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ApiScopeDto>> GetUserApiScopesAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Group)
                .ThenInclude(g => g.GroupApiScopes)
                .ThenInclude(gas => gas.ApiScope)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            return user.Group.GroupApiScopes
                .Select(gas => new ApiScopeDto
                {
                    Name = gas.ApiScope.Name,
                    Route = gas.ApiScope.Route,
                    Description = gas.ApiScope.Description
                }).ToList();
        }
    }
}
