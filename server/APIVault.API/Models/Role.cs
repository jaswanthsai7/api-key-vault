using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APIVault.API.Models
{
    public class Role
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        [JsonIgnore] // Prevent Role → Users → Role loop
        public ICollection<User> Users { get; set; }
    }
}
