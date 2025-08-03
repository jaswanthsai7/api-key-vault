using APIVault.API.DTOs.AuditLog;

namespace APIVault.API.Services.Interfaces
{
    public interface IAuditLogService
    {
        Task LogAsync(string apiKey, string endpoint, bool isKeyActive, Guid userId, string ip, int? statusCode = null);
        Task<List<ApiAuditLogDto>> GetLogsForUserAsync(Guid userId);
        Task<List<ApiAuditLogDto>> GetAllLogsAsync(); // Admin use
        Task<AuditStatsDto> GetUserStatsAsync(Guid userId);
    }
}
