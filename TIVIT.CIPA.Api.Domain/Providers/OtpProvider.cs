using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OtpNet;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TIVIT.CIPA.Api.Domain.Settings;

namespace TIVIT.CIPA.Api.Domain.Providers
{
    public class OtpProvider
    {
        private const int Step = 120;
        private const int Size = 6;
        private const int SecretLength = 20;
        private const OtpHashMode HashMode = OtpHashMode.Sha1;

        private readonly AuthSettings _authSettings;

        public OtpProvider(IOptionsMonitor<AuthSettings> authSettings)
        {
            _authSettings = authSettings.CurrentValue;
        }

        public (string secret, string otp) GenerateOtp()
        {
            string secret = GenerateSecret();
            var secretBytes = Base32Encoding.ToBytes(secret);

            var totp = GetTotp(secretBytes);

            return (secret, totp.ComputeTotp());
        }

        public bool VerifyOtp(string otp, string secret)
        {
            var totp = GetTotp(Base32Encoding.ToBytes(secret));
            return totp.VerifyTotp(DateTime.UtcNow, otp, out _, VerificationWindow.RfcSpecifiedNetworkDelay);
        }

        private Totp GetTotp(byte[] secret) => new(secret, mode: HashMode, step: Step, totpSize: Size, timeCorrection: new TimeCorrection(DateTime.UtcNow));

        public string GenerateToken(string upn, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_authSettings.OtpTokenSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("sub", upn),
                    new Claim("key", secret)
                }),

                Expires = GetExpiresUtc().DateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public (string id, string secret) GetIdentifierFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return default;

            var key = System.Text.Encoding.ASCII.GetBytes(_authSettings.OtpTokenSecret);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = null;
            try
            {
                principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            }
            catch (SecurityTokenSignatureKeyNotFoundException ex)
            {
                return default;
            }

            var id = principal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault();
            var secret = principal.Claims.Where(x => x.Type == "key").Select(x => x.Value).FirstOrDefault();

            return (id, secret);
        }

        static string GenerateSecret()
        {
            var secret = KeyGeneration.GenerateRandomKey(SecretLength);
            return Base32Encoding.ToString(secret);
        }

        static DateTimeOffset GetExpiresUtc() => DateTimeOffset.UtcNow.AddMinutes(2);
    }
}
