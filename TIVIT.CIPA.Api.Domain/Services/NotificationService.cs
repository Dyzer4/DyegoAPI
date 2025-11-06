using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using TIVIT.CIPA.Api.Domain.Interfaces.Services;
using TIVIT.CIPA.Api.Domain.Settings;

namespace TIVIT.CIPA.Api.Domain.Services
{
    public class NotificationService(
        IOptions<NotificationSettings> settings,
        ILogger<NotificationService> logger) : INotificationService
    {
        private readonly NotificationSettings _settings = settings.Value;
        private readonly ILogger<NotificationService> _logger = logger;

        private class Email
        {
            public string Subject { get; set; }
            public bool IsHtml { get; set; }
            public string Content { get; set; }
            public string From { get; set; }
            public string[] To { get; set; }
            public string[] Cc { get; set; }
            public string[] Bcc { get; set; }
        }

        public async Task<bool> SendEmailAsync(string content, string emailTo, string subject, bool sendToSupport)
        {
            var sent = true;

            if (_settings.IsDevEnv())
                emailTo = _settings.TestEmailTo;

            try
            {
                var email = new Email
                {
                    Subject = subject,
                    IsHtml = true,
                    Content = content,
                    From = _settings.EmailFrom,
                    To = [emailTo],
                    Cc = sendToSupport ? [_settings.SupportEmailTo] : [],
                    Bcc = [_settings.EmailFrom]
                };

                await SendAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Não foi possível enviar o email para {emailTo}.");
                sent = false;
            }

            return sent;
        }

        private async Task SendAsync(Email email)
        {
            _logger.LogDebug($"Enviando e-mail de ({email.From}) para ({string.Join(",", email.To)}). assunto {email.Subject}");

            _logger.LogDebug("Setando parametros para MailMessage Object");
            MailMessage mail = new MailMessage();
            mail.Priority = MailPriority.Normal;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = email.IsHtml;
            mail.Body = email.Content;
            mail.Subject = email.Subject;
            mail.From = new MailAddress(email.From);

            //TO
            if (email.To != null)
            {
                foreach (var recipient in email.To)
                {
                    mail.To.Add(recipient);
                }
            }

            //CC
            if (email.Cc != null)
            {
                foreach (var recipient in email.Cc)
                {
                    mail.CC.Add(recipient);
                }
            }

            //BCC
            if (email.Bcc != null)
            {
                foreach (var recipient in email.Bcc)
                {
                    mail.Bcc.Add(recipient);
                }
            }

            _logger.LogDebug("Tentando enviar email");

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Port = Convert.ToInt32(_settings.Port);
            smtpClient.Host = _settings.Server;
            smtpClient.Credentials = null;
            smtpClient.EnableSsl = false;
            await smtpClient.SendMailAsync(mail);

            _logger.LogDebug("E-mail enviado");
        }
    }
}
