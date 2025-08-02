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
        private readonly IConfiguration _config;

        public ApiKeyService(AppDbContext context,IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> GenerateApiKeyAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Group)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found.");

            // 1. Get allowed API scopes based on user's group
            var allowedScopeIds = await _context.GroupApiScopes
                .Where(gas => gas.GroupId == user.GroupId)
                .Select(gas => gas.ApiScopeId)
                .ToListAsync();

            var allowedScopes = await _context.ApiScopes
                .Where(scope => allowedScopeIds.Contains(scope.Id))
                .ToListAsync();

            if (!allowedScopes.Any())
                throw new Exception("No API scopes assigned to this user’s group.");

            // 2. Generate the API key string
            string apiKeyString = ApiKeyGenerator.Generate();

            // 3. Create new ApiKey
            var newApiKey = new ApiKey
            {
                Key = apiKeyString,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(_config.GetValue<int>("ApiKey:LifetimeDays")),
                CreatedAt = DateTime.UtcNow
            };

            _context.ApiKeys.Add(newApiKey);
            await _context.SaveChangesAsync();

            // 4. Link this API key to the allowed scopes
            foreach (var scope in allowedScopes)
            {
                _context.ApiKeyScopes.Add(new ApiKeyScope
                {
                    ApiKeyId = newApiKey.Id,
                    ApiScopeId = scope.Id
                });
            }

            await _context.SaveChangesAsync();

            return apiKeyString;
        }
    }
}
