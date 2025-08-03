using System;

namespace APIVault.API.DTOs.AuditLog
{
    public class ApiAuditLogDto
    {
        public string ApiKey { get; set; }
        public string Endpoint { get; set; }
        public bool IsKeyActive { get; set; }
        public Guid? UserId { get; set; }
        public string UserEmail { get; set; }
        public string IpAddress { get; set; }
        public int? StatusCode { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
