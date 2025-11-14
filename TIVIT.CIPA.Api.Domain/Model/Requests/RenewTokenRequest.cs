namespace TIVIT.CIPA.Api.Domain.Model.Requests
{
    public record RenewTokenRequest(string AccessToken, string RefreshToken);
}