using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APIVault.API.Models
{
    public class ApiScope
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Route { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public ICollection<ApiKeyScope> ApiKeyScopes { get; set; }

        [JsonIgnore]
        public ICollection<GroupApiScope> GroupApiScopes { get; set; }
    }
}
