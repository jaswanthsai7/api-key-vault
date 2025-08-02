using System;
using System.Collections.Generic;

namespace APIVault.API.Models
{
    public class ApiScope
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }         // e.g. "Weather API"
        public string Route { get; set; }        // e.g. "/api/weather"
        public string Description { get; set; }

        public ICollection<ApiKeyScope> ApiKeyScopes { get; set; }
        public ICollection<GroupApiScope> GroupApiScopes { get; set; }
    }
}
