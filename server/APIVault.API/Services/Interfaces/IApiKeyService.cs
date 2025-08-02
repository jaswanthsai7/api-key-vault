namespace APIVault.API.Services.Interfaces
{
    public interface IApiKeyService
    {
        Task<string> GenerateApiKeyAsync(Guid userId);

    }
}
