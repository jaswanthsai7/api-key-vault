using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APIVault.API.Models
{
    public class ApiKey
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Key { get; set; }

        public Guid UserId { get; set; }

        [JsonIgnore] // Prevent ApiKey → User → ApiKeys → ApiKey loop
        public User User { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;

        public ICollection<ApiKeyScope> ApiKeyScopes { get; set; }
    }
}
