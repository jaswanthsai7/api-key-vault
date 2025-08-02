namespace APIVault.API.Models
{
    public class ApiKeyScope
    {
        public int Id { get; set; }

        public int ApiKeyId { get; set; }
        public ApiKey ApiKey { get; set; }

        public int ApiScopeId { get; set; }
        public ApiScope ApiScope { get; set; }
    }
}
