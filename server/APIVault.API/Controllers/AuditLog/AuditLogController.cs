using APIVault.API.Services.Implementations;
using APIVault.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APIVault.API.Controllers.AuditLog
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;
        private readonly IUserService _userService;

        public AuditLogController(IAuditLogService auditLogService, IUserService userService)
        {
            _auditLogService = auditLogService;
            _userService = userService;
        }

        // For logged-in users to view their own logs
        [HttpGet("my")]
        public async Task<IActionResult> GetMyLogs()
        {
            var userIdClaim = User.FindFirst("userId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = Guid.Parse(userIdClaim.Value);

            var logs = await _auditLogService.GetLogsForUserAsync(userId);
            return Ok(logs);
        }

        // For admin to view all logs
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllLogs()
        {
            var logs = await _auditLogService.GetAllLogsAsync();
            return Ok(logs);
        }

        [HttpGet("stats")]
        [Authorize]
        public async Task<IActionResult> GetStats()
        {
            var userIdClaim = User.FindFirst("userId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            var userId = Guid.Parse(userIdClaim.Value);
            var stats = await _auditLogService.GetUserStatsAsync(userId);
            return Ok(stats);
        }


    }
}
