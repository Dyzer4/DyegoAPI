using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using TIVIT.CIPA.Api.Domain.Settings;
using Encod = System.Text.Encoding;

namespace TIVIT.CIPA.Api.Domain.Providers
{
    public class PasswordProvider
    {
        private readonly AuthSettings _authSettings;

        public PasswordProvider(IOptionsMonitor<AuthSettings> authSettings)
        {
            _authSettings = authSettings.CurrentValue;
        }
        public byte[] CreatePasswordHash(string password)
        {
            using var hmac = new HMACSHA512(Encod.Unicode.GetBytes(_authSettings.PasswordSalt));
            return hmac.ComputeHash(Encod.UTF8.GetBytes(password));
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash)
        {
            using var hmac = new HMACSHA512(Encod.Unicode.GetBytes(_authSettings.PasswordSalt));
            var computedHash = hmac.ComputeHash(Encod.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        public static (string key, DateTimeOffset expiresUtc) GeneratePasswordKey(int expiresInMinutes)
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            string base64 = Convert.ToBase64String(randomBytes);
            string base64Url = base64.Replace('+', '-').Replace('/', '_').TrimEnd('=');

            return (base64Url, GetExpiresUtc(expiresInMinutes));
        }

        private static DateTimeOffset GetExpiresUtc(int expiresIn) => DateTimeOffset.UtcNow.AddMinutes(expiresIn);
    }
}
