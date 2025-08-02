namespace APIVault.API.Models
{
    public class GroupApiScope
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int ApiScopeId { get; set; }
        public ApiScope ApiScope { get; set; }
    }
}
