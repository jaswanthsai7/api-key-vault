using Microsoft.EntityFrameworkCore;
using APIVault.API.Data;

namespace APIVault.API.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        // Define routes that should bypass API key check
        private static readonly List<string> _bypassPaths = new()
    {
        "/swagger",
        "/api/Auth",
        "/api/User",
        "/api/Admin",
        "/api/ApiKey"
    };

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AppDbContext db)
        {
            var requestPath = context.Request.Path.Value;

            // Skip API key check for specified routes
            if (_bypassPaths.Any(path => requestPath.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            string apiKey = null;

            // Try to get the API key from the Authorization header
            if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                if (authHeader.ToString().StartsWith("Bearer "))
                {
                    apiKey = authHeader.ToString()["Bearer ".Length..].Trim();
                }
            }

            // Fallback to query param
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                apiKey = context.Request.Query["api_key"];
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key is missing.");
                return;
            }

            // Validate API key from DB
            var keyEntity = await db.ApiKeys
                .Include(k => k.ApiKeyScopes)
                    .ThenInclude(s => s.ApiScope)
                .FirstOrDefaultAsync(k =>
                    k.Key == apiKey &&
                    !k.IsRevoked &&
                    (!k.ExpiresAt.HasValue || k.ExpiresAt > DateTime.UtcNow)
                );

            if (keyEntity == null)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Invalid or expired API key.");
                return;
            }

            // Attach useful info to context
            context.Items["ApiUserId"] = keyEntity.UserId;
            context.Items["ApiKey"] = keyEntity.Key;
            context.Items["ApiScopes"] = keyEntity.ApiKeyScopes
                .Select(s => s.ApiScope.Route)
                .ToList();

            await _next(context);
        }
    }

}
