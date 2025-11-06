using System.Net;

namespace TIVIT.CIPA.Api.Domain.MailContent
{
    internal class AuthMailContent
    {
        private readonly static string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates/Email");

        public static string GetOtpContent(string otp, string userName)
        {
            var templateEmail = Path.Combine(templatePath, "Otp.html");
            var content = new StreamReader(templateEmail).ReadToEnd();

            content = content.Replace("##USER_NAME##", userName);
            content = content.Replace("##OTP##", otp);

            return content;
        }

        public static string GetPasswordRecoverContent(string key, string userName, string link)
        {
            var templateEmail = Path.Combine(templatePath, "PasswordRecover.html");
            var content = new StreamReader(templateEmail).ReadToEnd();

            link += "auth/password-change?k=" + WebUtility.UrlEncode(key);

            content = content.Replace("##USER_NAME##", userName);
            content = content.Replace("##LINK##", link);

            return content;
        }
    }
}
