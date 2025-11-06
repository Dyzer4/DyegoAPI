using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Interfaces.Services;
using TIVIT.CIPA.Api.Domain.Interfaces.Services.Azure;
using TIVIT.CIPA.Api.Domain.Model.Services;
using TIVIT.CIPA.Api.Domain.Repositories;
using TIVIT.CIPA.Api.Domain.Resources;
using TIVIT.CIPA.Api.Domain.Settings;

namespace TIVIT.CIPA.Api.Domain.Services
{
    public class EmailService : IEmailService
    {
        const string QUEUE_NAME = "notifications-email";

        private readonly static string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");

        private readonly INotificationService _notificationService;
        private readonly ILogger<EmailService> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly NotificationSettings _notificationSettings;

        public EmailService(
            INotificationService notificationService,
            ILogger<EmailService> logger,
            IStringLocalizer<SharedResource> localizer,
            IOptions<NotificationSettings> notificationSettings
        )
        {
            _notificationService = notificationService;
            _logger = logger;
            _localizer = localizer;
            _notificationSettings = notificationSettings.Value;
        }

        public async Task SendUserFirstAccessContentAsync(UserFirstAccessEmailRequest request)
        {
            var subject = "Primeiro Acesso";
            try
            {
                var link = "https://exemplo.xpto/";

                var content = GetUserFirstAccessContent(request.Name, request.TempPassword, link);
                await _notificationService.SendEmailAsync(content, request.Email, subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Não foi possível enviar o email: {subject}");
            }
        }

        public static string GetUserFirstAccessContent(string name, string tempPassword, string link)
        {
            var templateEmail = Path.Combine($"{templatePath}/Email", "UserFirstAccess.html");
            var content = new StreamReader(templateEmail).ReadToEnd();

            content = content.Replace("##USER_NAME##", name);
            content = content.Replace("##TEMP_PASSWORD##", tempPassword);
            content = content.Replace("##LINK##", link);

            return content;
        }
    }
}
