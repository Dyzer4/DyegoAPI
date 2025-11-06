namespace TIVIT.CIPA.Api.Domain.Settings
{
    public class AuthSettings
    {
        public string PasswordSalt { get; set; }
        public string OtpTokenSecret { get; set; }
        public string SecTokenSecret { get; set; }
    }
}
