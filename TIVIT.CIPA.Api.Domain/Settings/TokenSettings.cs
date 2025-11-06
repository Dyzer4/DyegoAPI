namespace TIVIT.CIPA.Api.Domain.Settings
{
    public class TokenSettings
    {
        public string ClientId { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Expires { get; set; }
        public string Secret { get; set; }
    }
}