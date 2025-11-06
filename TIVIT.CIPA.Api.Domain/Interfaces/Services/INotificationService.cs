namespace TIVIT.CIPA.Api.Domain.Interfaces.Services
{
    public interface INotificationService
    {
        Task<bool> SendEmailAsync(string content, string emailTo, string subject, bool sendToSupport = false);
    }
}
