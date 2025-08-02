namespace APIVault.API.Models
{
    public class ApiKey
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<ApiKeyScope> ApiKeyScopes { get; set; }
    }
}
