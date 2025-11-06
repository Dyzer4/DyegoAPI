using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Polly;
using System.Globalization;
using TIVIT.CIPA.Api.Domain.Interfaces.Services.Azure;
using TIVIT.CIPA.Api.Domain.Settings;

namespace TIVIT.CIPA.Api.Domain.Services.Azure
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;

        public AuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetTokenAsync(string clientId, string clientSecret, string resource)
        {
            string token = null;

            try
            {
                await Policy
                    .Handle<AdalException>(ex => ex.ErrorCode == "temporarily_unavailable" || ex.ErrorCode == "service_unavailable")
                    .WaitAndRetry(3, a => TimeSpan.FromSeconds(3))
                    .Execute(async () =>
                    {
                        var authority = string.Format(CultureInfo.InvariantCulture, _configuration["AzureAd:Authority"], _configuration["AzureAd:Tenant"]);
                        var authContext = new AuthenticationContext(authority, false, null, new CustomHttpClientFactory());
                        var credential = new ClientCredential(clientId, clientSecret);

                        var result = await authContext.AcquireTokenAsync(resource, credential);

                        token = result.AccessToken;
                    });
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao autenticar aplicação no recurso solicitado.", ex);
            }

            return token;
        }
    }
}
