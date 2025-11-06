using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace TIVIT.CIPA.Api.Extensions
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            var config = builder.Configuration;

            var key = System.Text.Encoding.ASCII.GetBytes(config["Token:Secret"]);

            return builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddJwtBearer("AzureAd", o =>
            {
                o.Authority = string.Format(config["AzureAd:Authority"], config["AzureAd:Tenant"]);
                o.Audience = config["AzureAd:Audience"];
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = config["AzureAd:Issuer"]
                };
            });
        }
    }
}
