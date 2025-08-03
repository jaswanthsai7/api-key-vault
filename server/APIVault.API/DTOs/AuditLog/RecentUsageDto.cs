namespace APIVault.API.DTOs.AuditLog
{
    public class RecentUsageDto
    {
        public string Endpoint { get; set; }
        public string Date { get; set; }
        public int CallCount { get; set; }
    }

}
