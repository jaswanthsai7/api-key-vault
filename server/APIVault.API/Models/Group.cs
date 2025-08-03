using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APIVault.API.Models
{
    public class Group
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        [JsonIgnore] //  Prevent Group → Users → Group loop
        public ICollection<User> Users { get; set; }

        public ICollection<GroupApiScope> GroupApiScopes { get; set; }
    }
}
