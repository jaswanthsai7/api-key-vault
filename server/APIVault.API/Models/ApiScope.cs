namespace APIVault.API.Models
{
    public class ApiScope
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;

        public ICollection<GroupApiScope> GroupApiScopes { get; set; }
        public ICollection<ApiKeyScope> ApiKeyScopes { get; set; }
    }
}
