using System;
using System.Collections.Generic;

namespace APIVault.API.Models
{
    public class ApiKey
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Key { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }  // Optional expiration
        public bool IsRevoked { get; set; } = false;

        public ICollection<ApiKeyScope> ApiKeyScopes { get; set; }
    }
}
