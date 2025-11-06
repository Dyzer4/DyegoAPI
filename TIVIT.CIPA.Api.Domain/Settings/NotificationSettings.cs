namespace TIVIT.CIPA.Api.Domain.Settings
{
    public class NotificationSettings
    {
        private const string DEV_ENV = "DEV";

        public string UrlPostman { get; set; }
        public string EmailFrom { get; set; }
        public string TestEmailTo { get; set; }
        public string SupportEmailTo { get; set; }
        public string Environment { get; set; } = DEV_ENV;
        public int Port { get; set; }
        public string Server { get; set; }
        public int CompanyIdTIVIT { get; set; }
        public string UrlInternal { get; set; }
        public string UrlPublic { get; set; }

        public bool IsDevEnv()
        {
            return Environment == DEV_ENV;
        }
    }
}
