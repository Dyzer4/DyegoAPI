using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TIVIT.CIPA.Api.Domain.Providers;

namespace TIVIT.CIPA.Api.Domain.Extensions
{
    public static class IdentityExtensions
    {
        private const string AuthHeaderKey = "x-security";

        public static string GetRole(this HttpContext httpContext)
        {
            var payload = httpContext.GetPayload();

            return payload?.Role;
        }

        private static Payload GetPayload(this HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue(AuthHeaderKey, out var authToken);

            if (string.IsNullOrEmpty(authToken)) return null;

            var service = httpContext.GetAuthService();
            return service.Decode(authToken, out _);
        }

        private static AuthTokenProvider GetAuthService(this HttpContext httpContext)
            => httpContext.RequestServices.GetService<AuthTokenProvider>();
    }
}
