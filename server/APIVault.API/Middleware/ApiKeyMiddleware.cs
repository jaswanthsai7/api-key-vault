using APIVault.API.Data;
using APIVault.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
            "/api/ApiKey",
            "/api/AuditLog",
            "/api/Group",
            "/api/Role"
        };

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AppDbContext db, IAuditLogService logger)
        {
            var requestPath = context.Request.Path.Value;
            var ip = context.Connection.RemoteIpAddress?.ToString();
            string apiKey = null;
            Guid userId = Guid.Empty;
            bool isValidKey = false;

            try
            {
                // Bypass certain routes
                if (_bypassPaths.Any(path => requestPath.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
                {
                    await _next(context);
                    return;
                }

                // Try Authorization header (Bearer token)
                if (context.Request.Headers.TryGetValue("Authorization", out var authHeader) &&
                    authHeader.ToString().StartsWith("Bearer "))
                {
                    apiKey = authHeader.ToString()["Bearer ".Length..].Trim();
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
                    await logger.LogAsync(null, requestPath, false, userId, ip, 401);
                    return;
                }

                // Lookup key
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
                    await logger.LogAsync(apiKey, requestPath, false, userId, ip, 403);
                    return;
                }

                // NEW: Extract JWT from cookie instead of header
                var tokenStr = context.Request.Cookies["access_token"];
                if (!string.IsNullOrEmpty(tokenStr))
                {
                    var handler = new JwtSecurityTokenHandler();
                    if (handler.CanReadToken(tokenStr))
                    {
                        var jwtToken = handler.ReadJwtToken(tokenStr);
                        var userIdClaim = jwtToken.Claims.FirstOrDefault(c =>
                            c.Type == ClaimTypes.NameIdentifier || c.Type == "sub" || c.Type == "name");

                        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var parsedUserId))
                        {
                            userId = parsedUserId;
                        }
                    }
                }
                else
                {
                    // fallback to keyEntity owner
                    userId = keyEntity.UserId;
                }

                // Attach useful info to context
                isValidKey = true;

                context.Items["ApiUserId"] = userId;
                context.Items["ApiKey"] = keyEntity.Key;
                context.Items["ApiScopes"] = keyEntity.ApiKeyScopes
                    .Select(s => s.ApiScope.Route)
                    .ToList();

                await _next(context);

                await logger.LogAsync(apiKey, requestPath, true, userId, ip, context.Response.StatusCode);
            }
            catch (Exception)
            {
                await logger.LogAsync(apiKey, requestPath, isValidKey, userId, ip, 500);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong in API middleware.");
            }
        }


    }
}
