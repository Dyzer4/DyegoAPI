namespace TIVIT.CIPA.Api.Domain.Model.Responses
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool FirstAccess { get; set; }

        public AuthResponse(string accessToken, string refreshToken, bool firstAccess)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            FirstAccess = firstAccess;
        }
    }
}
