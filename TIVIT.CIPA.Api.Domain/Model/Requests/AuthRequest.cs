namespace TIVIT.CIPA.Api.Domain.Model.Requests
{
    public record AuthRequest(string Username, string Password);
    public record RenewTokenRequest(string AccessToken, string RefreshToken);
}
