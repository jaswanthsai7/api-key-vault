using System.Text.Json.Serialization;
using System;

namespace APIVault.API.Models
{
    public class ApiKeyScope
    {
        public Guid ApiKeyId { get; set; }

        [JsonIgnore] //  Prevent ApiKeyScope ? ApiKey ? ApiKeyScopes loop
        public ApiKey ApiKey { get; set; }

        public Guid ApiScopeId { get; set; }
        public ApiScope ApiScope { get; set; }
    }
}
