using System;

namespace APIVault.API.Models
{
    public class ApiAuditLog
    {
        public int Id { get; set; }

        public string ApiKey { get; set; }

        public string Endpoint { get; set; }

        public bool IsKeyActive { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string IpAddress { get; set; }

        public int? StatusCode { get; set; }
    }
}
