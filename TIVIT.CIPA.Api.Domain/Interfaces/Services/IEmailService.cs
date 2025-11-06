using TIVIT.CIPA.Api.Domain.Model.Services;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendUserFirstAccessContentAsync(UserFirstAccessEmailRequest request);
    }
}