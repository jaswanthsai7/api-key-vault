using System.Security.Cryptography;
using System.Text;

namespace APIVault.API.Helpers
{
    public class EncryptionHelper
    {
        private readonly string _key;

        public EncryptionHelper(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(_key))
                throw new InvalidOperationException("Encryption key must be set.");
        }

        public string Decrypt(string encryptedText)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_key);
            var iv = new byte[16]; // 16-byte zero IV

            var encryptedBytes = Convert.FromBase64String(encryptedText);
            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(encryptedBytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}
