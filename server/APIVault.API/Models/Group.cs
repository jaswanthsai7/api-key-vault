using System;
using System.Collections.Generic;

namespace APIVault.API.Models
{
    public class Group
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<GroupApiScope> GroupApiScopes { get; set; }
    }
}
