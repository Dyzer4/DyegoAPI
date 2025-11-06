namespace TIVIT.CIPA.Api.Domain.Interfaces.Services.Azure
{
    public interface IAuthenticationService
    {
        Task<string> GetTokenAsync(string clientId, string clientSecret, string resource);
    }
}
