using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Model.Responses;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Business
{
    public interface IAuthBusiness
    {
        Task<Response<string>> GenerateOtpAsync(AuthRequest request);
        Task<Response<AuthResponse>> LogInAsync(OtpRequest request);
        Task<Response<string>> GetAuthTokenAsync();
        Task<Response<AuthResponse>> RenewRefreshTokenAsync(RenewTokenRequest request);
        Task LogOutAsync();
        Task SavePasswordAsync(AuthRequest request);
        Task<Response> RecoverPasswordAsync(string username);
        Task<Response> ChangePasswordAsync(ChangePasswordRequest request);
        Task<Response> ChangeFirstAccessPasswordAsync(FirstAccessPasswordRequest request);
        Task<Response<(int userId, double secondsToExpire)>> CheckPasswordRecoverKeyAsync(string passwordRecoverKey);
        Task<Response<ExternalAuthResponse>> GenerateServiceTokenAsync(ExternalAuthRequest request);
    }
}
