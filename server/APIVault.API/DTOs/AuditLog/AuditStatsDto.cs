namespace APIVault.API.DTOs.AuditLog
{
    public class AuditStatsDto
    {
        public int TotalApiKeys { get; set; }
        public int ActiveApiKeys { get; set; }
        public int ExpiredApiKeys { get; set; }
        public int ApiCallsLast30Days { get; set; }
        public List<RecentUsageDto> RecentUsage { get; set; }
    }

}
