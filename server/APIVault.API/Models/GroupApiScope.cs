using System;

namespace APIVault.API.Models
{
    public class GroupApiScope
    {
        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        public Guid ApiScopeId { get; set; }
        public ApiScope ApiScope { get; set; }
    }
}
