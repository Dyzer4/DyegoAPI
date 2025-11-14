using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using TIVIT.CIPA.Api.Domain.Interfaces.Business;
using TIVIT.CIPA.Api.Domain.Interfaces.Models;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Interfaces.Services;
using TIVIT.CIPA.Api.Domain.MailContent;
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Model.Responses;
using TIVIT.CIPA.Api.Domain.Providers;
using TIVIT.CIPA.Api.Domain.Repositories;
using TIVIT.CIPA.Api.Domain.Resources;
using TIVIT.CIPA.Api.Domain.Settings;
using TIVIT.CIPA.Api.Domain.Validators;

namespace TIVIT.CIPA.Api.Domain.Business
{
    public class AuthBusiness : IAuthBusiness
    {
        private readonly IUserInfo _userInfo;
        private readonly IAuthRepository _authRepository;
        private readonly TokenProvider _tokenProvider;
        private readonly OtpProvider _otpProvider;
        private readonly PasswordProvider _passwordProvider;
        private readonly AuthTokenProvider _authTokenProvider;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly NotificationSettings _notificationSettings;

        public AuthBusiness(
            IUserInfo userInfo,
            IAuthRepository authRepository,
            TokenProvider tokenProvider,
            OtpProvider otpProvider,
            PasswordProvider passwordProvider,
            AuthTokenProvider authTokenProvider,
            IStringLocalizer<SharedResource> localizer,
            IUserRepository userRepository,
            INotificationService notificationService,
            IOptions<NotificationSettings> notificationSettings)
        {
            _userInfo = userInfo;
            _authRepository = authRepository;
            _tokenProvider = tokenProvider;
            _otpProvider = otpProvider;
            _passwordProvider = passwordProvider;
            _authTokenProvider = authTokenProvider;
            _localizer = localizer;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _notificationSettings = notificationSettings.Value;
        }

        public async Task<Response<string>> GenerateOtpAsync(AuthRequest request)
        {
            var response = new Response<string>();

            var password = _passwordProvider.CreatePasswordHash(request.Password);

            var user = await _userRepository.GetFirstOrDefaultAsync(
                x => x.Email == request.Username && x.IsActive);

            if (user == null)
            {
                response.AddMessage("Usuário não encontrado.");
                return response;
            }

            var userAuth = await _authRepository.GetFirstOrDefaultAsync(
                x => x.UserId == user.Id && x.Password == password);

            if (userAuth == null)
            {
                response.AddMessage("Usuário e/ou senha inválidos.");
                return response;
            }

            var (secret, otp) = _otpProvider.GenerateOtp();

            var sent = await SendEmailOtpContent(otp, user.Email, user.FullName);
            if (!sent)
            {
                response.AddMessage("Não foi possível enviar o código de verificação: Serviço indisponível.");
                return response;
            }

            response.Data = _otpProvider.GenerateToken(request.Username, secret);

            return response;
        }

        public async Task<Response<AuthResponse>> LogInAsync(OtpRequest request)
        {
            var response = new Response<AuthResponse>();

            var (id, secret) = _otpProvider.GetIdentifierFromToken(request.Token);

            var validOtp = _otpProvider.VerifyOtp(request.Otp, secret);
            if (!validOtp)
            {
                response.AddMessage("Código OTP inválido.");
                return response;
            }

            var user = await _userRepository.GetFirstOrDefaultAsync(
                x => x.Email == id && x.IsActive);

            if (user == null)
            {
                response.AddMessage("Usuário inválido.");
                return response;
            }

            var userAuth = await _authRepository.GetFirstOrDefaultAsync(
                x => x.UserId == user.Id);

            var accessToken = _tokenProvider.GenerateToken(user.Email);
            var refreshToken = await GenerateRefreshTokenAsync(user.Id);

            response.Data = new AuthResponse(accessToken, refreshToken, userAuth?.FirstAccess ?? true);

            return response;
        }

        public async Task<Response<string>> GetAuthTokenAsync()
        {
            var response = new Response<string>();

            var user = await _userRepository.GetByNetworkUserAsync(_userInfo.Upn);

            ArgumentNullException.ThrowIfNull(user);

            response.Data = await GenerateAuthTokenAsync(user.Id);

            return response;
        }

        public async Task<Response<AuthResponse>> RenewRefreshTokenAsync(RenewTokenRequest request)
        {
            var response = new Response<AuthResponse>();

            var username = _tokenProvider.GetIdentifierFromToken(request.AccessToken);

            var user = await _userRepository.GetFirstOrDefaultAsync(
                x => x.Email == username && x.IsActive);

            if (user == null)
            {
                response.AddMessage("Usuário inválido");
                return response;
            }

            var userAuth = await _authRepository.GetFirstOrDefaultAsync(
                x => x.UserId == user.Id);

            var validRefreshToken = await ValidateRefreshTokenAsync(user.Id, request.RefreshToken);
            if (!validRefreshToken)
            {
                response.AddMessage("Token inválido.");
                return response;
            }

            var newAccessToken = _tokenProvider.GenerateToken(username);
            var newRefreshToken = await GenerateRefreshTokenAsync(user.Id);

            response.Data = new AuthResponse(newAccessToken, newRefreshToken, userAuth.FirstAccess);

            return response;
        }

        public async Task LogOutAsync()
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(
                x => x.Email.Equals(_userInfo.Upn, StringComparison.CurrentCultureIgnoreCase));

            if (user == null) return;

            var userAuth = await _authRepository.GetFirstOrDefaultAsync(
                x => x.UserId == user.Id);

            if (userAuth != null)
            {
                userAuth.RefreshToken = null;
                userAuth.LastLogin = DateTime.UtcNow;

                await _authRepository.UpdateAsync(userAuth);
            }
        }

        public async Task SavePasswordAsync(AuthRequest request)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(
                x => x.Email == request.Username && x.IsActive);

            if (user != null)
            {
                var userAuth = await _authRepository.GetFirstOrDefaultAsync(
                    x => x.UserId == user.Id);

                if (userAuth == null) return;

                var password = _passwordProvider.CreatePasswordHash(request.Password);

                userAuth.Password = password;
                userAuth.FirstAccess = false;

                await _authRepository.UpdateAsync(userAuth);
            }
        }

        public async Task<Response> RecoverPasswordAsync(string username)
        {
            var response = new Response();

            var user = await _userRepository.GetFirstOrDefaultAsync(
                x => x.Email == username && x.IsActive);

            if (user == null) return response;

            var (key, expiresUtc) = PasswordProvider.GeneratePasswordKey(5);

            var userAuth = await _authRepository.GetFirstOrDefaultAsync(
                x => x.UserId == user.Id);

            if (userAuth == null)
            {
                userAuth = UserAuth.CreatePasswordRecover(user.Id, key, expiresUtc);
                await _authRepository.CreateAsync(userAuth);
            }
            else
            {
                userAuth.UpdatePasswordRecover(key, expiresUtc);
                await _authRepository.UpdateAsync(userAuth);
            }

            var sent = await SendEmailPasswordRecoverContent(key, user.Email, user.FullName);
            if (!sent)
            {
                response.AddMessage("Não foi possível solicitar alteração de senha: Serviço indisponível.");
                return response;
            }

            return response;
        }

        public async Task<Response> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var response = new Response();

            var checkResponse = await CheckPasswordRecoverKeyAsync(request.Key);
            if (checkResponse.HasErrors)
            {
                response.AddMessage(checkResponse.Messages);
                return response;
            }

            var (userId, secondsToExpire) = checkResponse.Data;

            var validator = new PasswordValidator(_localizer);
            validator.Validate(request);
            if (!validator.IsValid)
            {
                response.AddMessage(validator.Notifications.Select(x => x.Message));
                return response;
            }

            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                var userAuth = await _authRepository.GetFirstOrDefaultAsync(
                    x => x.UserId == user.Id);

                if (userAuth == null) return response;

                var password = _passwordProvider.CreatePasswordHash(request.Password);

                userAuth.Password = password;

                await _authRepository.UpdateAsync(userAuth);
            }

            return response;
        }

        public async Task<Response> ChangeFirstAccessPasswordAsync(FirstAccessPasswordRequest request)
        {
            var response = new Response();

            var validator = new PasswordValidator(_localizer);
            validator.Validate(request);
            if (!validator.IsValid)
            {
                response.AddMessage(validator.Notifications.Select(x => x.Message));
                return response;
            }

            var user = await _userRepository.GetFirstOrDefaultAsync(
                u => u.Email.ToLower() == _userInfo.Upn.ToLower() && u.IsActive);

            if (user != null)
            {
                var userAuth = await _authRepository.GetFirstOrDefaultAsync(
                    x => x.UserId == user.Id);

                if (userAuth == null) return response;

                var password = _passwordProvider.CreatePasswordHash(request.Password);

                userAuth.Password = password;
                userAuth.FirstAccess = false;

                await _authRepository.UpdateAsync(userAuth);
            }

            return response;
        }

        public async Task<Response<(int userId, double secondsToExpire)>> CheckPasswordRecoverKeyAsync(
            string passwordRecoverKey)
        {
            var response = new Response<(int userId, double secondsToExpire)>();

            var userAuth = await _authRepository.GetFirstOrDefaultAsync(
                x => x.PasswordRecoverKey == passwordRecoverKey);

            if (userAuth == null)
            {
                response.AddMessage("Chave inválida.");
                return response;
            }

            var secondsToExpire = (userAuth.PasswordRecoverKeyExp.HasValue
                ? (userAuth.PasswordRecoverKeyExp.Value - DateTime.UtcNow).TotalSeconds
                : -1);

            if (secondsToExpire <= 0)
            {
                response.AddMessage("Chave expirada. Faça uma nova solicitação para recuperação de senha.");
                return response;
            }

            response.Data = (userAuth.UserId, secondsToExpire);

            return response;
        }

        public async Task<Response<ExternalAuthResponse>> GenerateServiceTokenAsync(ExternalAuthRequest request)
        {
            var response = new Response<ExternalAuthResponse>();

            var user = await _userRepository.GetFirstOrDefaultAsync(
                x => x.Email == request.Username && x.IsActive);

            if (user == null)
            {
                response.AddMessage("Usuário não encontrado.");
                return response;
            }

            var password = _passwordProvider.CreatePasswordHash(request.Password);

            var userAuth = await _authRepository.GetFirstOrDefaultAsync(
                x => x.UserId == user.Id && x.Password == password);

            if (userAuth == null)
            {
                response.AddMessage("Usuário e/ou Senha inválido(s).");
                return response;
            }

            var accessToken = _tokenProvider.GenerateToken(user.Email);
            var securityToken = await GenerateAuthTokenAsync(user.Id);

            response.Data = new ExternalAuthResponse(accessToken, securityToken);

            return response;
        }

        private async Task<string> GenerateRefreshTokenAsync(int userId)
        {
            var (Token, ExpiresUtc) = _tokenProvider.GenerateRefreshToken();

            var userAuth = await _authRepository.GetFirstOrDefaultAsync(x => x.UserId == userId);
            if (userAuth == null)
            {
                userAuth = new()
                {
                    UserId = userId,
                    RefreshToken = Token,
                    IsActive = true
                };
                await _authRepository.CreateAsync(userAuth);
            }
            else
            {
                userAuth.RefreshToken = Token;
                userAuth.LastLogin = DateTime.UtcNow;

                await _authRepository.UpdateAsync(userAuth);
            }

            return Token;
        }

        private async Task<string> GenerateAuthTokenAsync(int userId)
        {
            var actionRes = await _userRepository.GetPermissionResult(userId);
            var expireDate = AuthTokenProvider.GetExpireDate();

            ArgumentNullException.ThrowIfNull(actionRes);

            var userAuth = await _authRepository.GetFirstOrDefaultAsync(
                x => x.UserId == actionRes.User.Id);

            var payload = new Payload(
                actionRes.User.Email,
                AuthTokenProvider.GetExp(expireDate),
                expireDate,
                actionRes.User.FullName,
                actionRes.Role.Code,
                actionRes.Actions?.Select(x => x.Name).ToArray(),
                actionRes.User.Id,
                (userAuth?.FirstAccess ?? true));

            return _authTokenProvider.Create(payload);
        }

        private async Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            var userAuth = await _authRepository.GetFirstOrDefaultAsync(
                x => x.UserId == userId && x.RefreshToken == refreshToken);

            // Se não existir ou RefreshTokenExp for null, retornar false
            return userAuth?.IsActive == true;
        }

        private async Task<bool> SendEmailOtpContent(string otp, string email, string userName)
        {
            var subject = "CIPA 2FA";
            var content = AuthMailContent.GetOtpContent(otp, userName);
            return await _notificationService.SendEmailAsync(content, email, subject);
        }

        private async Task<bool> SendEmailPasswordRecoverContent(string key, string email, string userName)
        {
            var subject = "CIPA - Recuperação de Senha";
            var link = "https://example.xpto";
            var content = AuthMailContent.GetPasswordRecoverContent(key, userName, link);
            return await _notificationService.SendEmailAsync(content, email, subject);
        }
    }
}
