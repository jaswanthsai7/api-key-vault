using APIVault.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace APIVault.API.Filters
{
    public class ApiKeyAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _requiredRoute;

        public ApiKeyAuthorizeAttribute(string requiredRoute)
        {
            _requiredRoute = requiredRoute;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var db = context.HttpContext.RequestServices.GetService(typeof(AppDbContext)) as AppDbContext;

            var request = context.HttpContext.Request;
            string apiKey = null;

            // Get API key from Authorization: Bearer <key>
            if (request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var header = authHeader.ToString();
                if (header.StartsWith("Bearer "))
                {
                    apiKey = header["Bearer ".Length..].Trim();
                }
            }

            // Fallback: api_key query param
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                apiKey = request.Query["api_key"];
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "API key is missing."
                };
                return;
            }

            // Lookup API key
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
                context.Result = new ContentResult
                {
                    StatusCode = 403,
                    Content = "Invalid or expired API key."
                };
                return;
            }

            // Check for required scope
            var allowedRoutes = keyEntity.ApiKeyScopes
                .Select(s => s.ApiScope.Route)
                .ToList();

            if (!allowedRoutes.Contains(_requiredRoute, StringComparer.OrdinalIgnoreCase))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 403,
                    Content = $"API key does not have access to {_requiredRoute}."
                };
                return;
            }

            // If needed, store info in HttpContext for controller
            context.HttpContext.Items["ApiUserId"] = keyEntity.UserId;
            context.HttpContext.Items["ApiScopes"] = allowedRoutes;
        }
    }
}
