using APIVault.API.Data;
using APIVault.API.DTOs;
using APIVault.API.DTOs.AuditLog;
using APIVault.API.Models;
using APIVault.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVault.API.Services.Implementations
{
    public class AuditLogService : IAuditLogService
    {
        private readonly AppDbContext _context;

        public AuditLogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(string apiKey, string endpoint, bool isKeyActive, Guid userId, string ip, int? statusCode = null)
        {
            var log = new ApiAuditLog
            {
                ApiKey = apiKey,
                Endpoint = endpoint,
                IsKeyActive = isKeyActive,
                UserId = userId,
                IpAddress = ip,
                StatusCode = statusCode,
                Timestamp = DateTime.UtcNow
            };

            _context.ApiAuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ApiAuditLogDto>> GetLogsForUserAsync(Guid userId)
        {
                    return await _context.ApiAuditLogs
            .Where(log => log.UserId == userId)
            .Include(log => log.User) // <-- this is crucial
            .OrderByDescending(log => log.Timestamp)
            .Select(log => new ApiAuditLogDto
            {
                ApiKey = log.ApiKey,
                Endpoint = log.Endpoint,
                IsKeyActive = log.IsKeyActive,
                UserEmail = log.User.Email,  // will now work
                IpAddress = log.IpAddress,
                StatusCode = log.StatusCode,
                Timestamp = log.Timestamp
            })
            .ToListAsync();

        }

        public async Task<List<ApiAuditLogDto>> GetAllLogsAsync()
        {
            return await _context.ApiAuditLogs
                .Include(log => log.User)
                .OrderByDescending(log => log.Timestamp)
                .Select(log => new ApiAuditLogDto
                {
                    ApiKey = log.ApiKey,
                    Endpoint = log.Endpoint,
                    IsKeyActive = log.IsKeyActive,
                    UserEmail = log.User.Email,
                    IpAddress = log.IpAddress,
                    StatusCode = log.StatusCode,
                    Timestamp = log.Timestamp
                })
                .ToListAsync();
        }

        public async Task<AuditStatsDto> GetUserStatsAsync(Guid userId)
        {
            var now = DateTime.UtcNow;
            var last30Days = now.AddDays(-30);

            var totalKeys = await _context.ApiKeys.CountAsync(k => k.UserId == userId);
            var activeKeys = await _context.ApiKeys.CountAsync(k =>
                k.UserId == userId &&
                !k.IsRevoked &&
                (!k.ExpiresAt.HasValue || k.ExpiresAt > now)
            );
            var expiredKeys = totalKeys - activeKeys;

            var apiCalls = await _context.ApiAuditLogs.CountAsync(log =>
                log.UserId == userId && log.Timestamp >= last30Days
            );

            // First bring raw grouped data into memory
            var rawUsage = await _context.ApiAuditLogs
                .Where(log => log.UserId == userId && log.Timestamp >= last30Days)
                .GroupBy(log => new { log.Endpoint, Date = log.Timestamp.Date })
                .Select(g => new
                {
                    g.Key.Endpoint,
                    g.Key.Date,
                    CallCount = g.Count()
                })
                .OrderByDescending(x => x.Date)
                .Take(10)
                .ToListAsync();

            // Then convert Date to string in-memory
            var recentUsage = rawUsage.Select(x => new RecentUsageDto
            {
                Endpoint = x.Endpoint,
                Date = x.Date.ToString("yyyy-MM-dd"),
                CallCount = x.CallCount
            }).ToList();

            return new AuditStatsDto
            {
                TotalApiKeys = totalKeys,
                ActiveApiKeys = activeKeys,
                ExpiredApiKeys = expiredKeys,
                ApiCallsLast30Days = apiCalls,
                RecentUsage = recentUsage
            };
        }


    }
}
