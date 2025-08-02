using System.Security.Cryptography;

namespace APIVault.API.Helpers
{
    public static class ApiKeyGenerator
    {
        public static string Generate(int length = 32)
        {
            // Generates a secure random API key (Base64 encoded)
            var randomBytes = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
