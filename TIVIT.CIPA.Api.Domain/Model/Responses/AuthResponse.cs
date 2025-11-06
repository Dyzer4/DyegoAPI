namespace TIVIT.CIPA.Api.Domain.Model.Responses
{
    public record AuthResponse(string AccessToken, string RefreshToken, bool UserFirstAccess);
}
