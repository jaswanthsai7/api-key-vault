using APIVault.API.Data;
using APIVault.API.Helpers;
using APIVault.API.Models;
using APIVault.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIVault.API.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly AppDbContext _context;

        public ApiKeyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateApiKeyAsync(Guid userId)
        {
            // 1. Get user with role and group info
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Group)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found.");

            // 2. Get allowed API scopes via group
            var allowedScopeIds = await _context.GroupApiScopes
                .Where(gas => gas.GroupId == user.GroupId)
                .Select(gas => gas.ApiScopeId)
                .ToListAsync();

            var allowedScopes = await _context.ApiScopes
                .Where(s => allowedScopeIds.Contains(s.Id))
                .ToListAsync();

            if (!allowedScopes.Any())
                throw new Exception("No API scopes assigned to this user’s group.");

            // 3. Generate API key
            string generatedKey = ApiKeyGenerator.Generate();

            var newApiKey = new ApiKey
            {
                Key = generatedKey,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30), // Optional: 30-day expiration
                IsRevoked = false
            };

            _context.ApiKeys.Add(newApiKey);
            await _context.SaveChangesAsync();

            // 4. Add mapping to allowed scopes
            foreach (var scope in allowedScopes)
            {
                var keyScope = new ApiKeyScope
                {
                    ApiKeyId = newApiKey.Id,
                    ApiScopeId = scope.Id
                };

                _context.ApiKeyScopes.Add(keyScope);
            }

            await _context.SaveChangesAsync();

            return generatedKey;
        }
    }
}
