namespace APIVault.API.DTOs.ApiKey
{
    public class ApiKeyResponseDto
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRevoked { get; set; }
    }
}
