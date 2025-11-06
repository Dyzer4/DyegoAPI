using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TIVIT.CIPA.Api.Domain.Settings;

namespace TIVIT.CIPA.Api.Domain.Providers
{
    public class TokenProvider
    {
        private readonly TokenSettings _tokenSettings;

        public TokenProvider(IOptionsMonitor<TokenSettings> tokenSettings)
        {
            _tokenSettings = tokenSettings.CurrentValue;
        }

        public string GenerateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_tokenSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("azp", _tokenSettings.ClientId),
                    new Claim("sub", username),
                    new Claim(ClaimTypes.Upn, username)
                }),

                Issuer = _tokenSettings.Issuer,
                Audience = _tokenSettings.Audience,
                Expires = GetExpiresUtc().DateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GetIdentifierFromToken(string token)
        {
            var key = System.Text.Encoding.ASCII.GetBytes(_tokenSettings.Secret);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault();
        }

        public (string Token, DateTimeOffset ExpiresUtc) GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return (Convert.ToBase64String(randomNumber), GetExpiresUtc());
        }

        private DateTimeOffset GetExpiresUtc() => DateTimeOffset.UtcNow.AddMinutes(_tokenSettings.Expires);
    }
}
