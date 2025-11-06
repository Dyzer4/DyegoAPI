using IHttpClientFactory = Microsoft.IdentityModel.Clients.ActiveDirectory.IHttpClientFactory;

namespace TIVIT.CIPA.Api.Domain.Services
{
    internal class CustomHttpClientFactory : IHttpClientFactory
    {
        public HttpClient GetHttpClient()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = delegate { return true; };
            return new HttpClient(httpClientHandler);
        }
    }
}
