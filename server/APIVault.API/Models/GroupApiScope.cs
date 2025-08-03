using System.Text.Json.Serialization;
using System;

namespace APIVault.API.Models
{
    public class GroupApiScope
    {
        public Guid GroupId { get; set; }

        [JsonIgnore] //  Avoid GroupApiScope → Group → GroupApiScopes loop
        public Group Group { get; set; }

        public Guid ApiScopeId { get; set; }
        public ApiScope ApiScope { get; set; }
    }
}
