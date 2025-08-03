using System;
using System.Collections.Generic;

namespace APIVault.API.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<ApiKey> ApiKeys { get; set; }
    }
}
