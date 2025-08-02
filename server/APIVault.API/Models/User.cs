
namespace APIVault.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public ICollection<ApiKey> ApiKeys { get; set; }
        public DateTime CreatedAt { get; internal set; }
    }
}
