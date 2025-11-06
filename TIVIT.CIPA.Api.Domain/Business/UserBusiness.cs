using ClosedXML.Excel;
using TIVIT.CIPA.Api.Domain.Interfaces.Business;
using TIVIT.CIPA.Api.Domain.Interfaces.Models;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Interfaces.Services;
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Model.Responses;
using TIVIT.CIPA.Api.Domain.Model.Services;
using TIVIT.CIPA.Api.Domain.Providers;
using TIVIT.CIPA.Api.Domain.Settings;

namespace TIVIT.CIPA.Api.Domain.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserInfo _userInfo;
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IProfileRepository _roleRepository;
        private readonly IUserPermissionRepository _permissionRepository;
        private readonly PasswordProvider _passwordProvider;
        private readonly IEmailService _emailService;

        public UserBusiness(
            IUserInfo userInfo,
            IUserRepository userRepository,
            IAuthRepository authRepository,
            IProfileRepository roleRepository,
            IUserPermissionRepository permissionRepository,
            PasswordProvider passwordProvider,
            IEmailService emailService)
        {
            _userInfo = userInfo;
            _userRepository = userRepository;
            _authRepository = authRepository;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _passwordProvider = passwordProvider;
            _emailService = emailService;
        }

        public async Task<IEnumerable<User>> GetByActiveAsync(bool active)
        {
            return await _userRepository.GetByFilterAsync(x => x.IsActive);
        }

        public async Task<Response<UserDetailResponse>> GetByIdAsync(int id)
        {
            var response = new Response<UserDetailResponse>();

            //if (_userInfo.Role == null)
            //{
            //    response.AddMessage("Usuário sem permissão.");
            //    return response;
            //}

            var user = await this._userRepository.GetByIdAsync(id);

            if (user == null)
                return response;

            var permissions = await _permissionRepository.GetByUserId(user.Id);

            response.Data = new UserDetailResponse()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                BirthDate = user.BirthDate,
                IsActive = user.IsActive,
                ProfileId = user.ProfileId,
                Actions = permissions.Select(x => x.ActionId)
            };

            return response;
        }

        public async Task<Response<int>> CreateAsync(UserCreateRequest createRequest)
        {
            var response = new Response<int>();

            //if (_userInfo.Role == null)
            //{
            //    response.AddMessage("Usuário sem permissão.");
            //    return response;
            //}

            var user = new User()
            {
                FullName = createRequest.FullName,
                Email = createRequest.Email,
                CorporateId = createRequest.CorporateId,
                BirthDate = createRequest.BirthDate,
                ProfileId = createRequest.ProfileId,
                CreateDate = DateTime.Now,
                CreateUser = _userInfo.Upn
            };

            try
            {

                await _userRepository.CreateAsync(user);

                var tempPassword = Guid.NewGuid().ToString().GetHashCode().ToString("x");

                var userAuth = new UserAuth();
                userAuth.UserId = user.Id;
                userAuth.FirstAccess = true;
                userAuth.Password = _passwordProvider.CreatePasswordHash(tempPassword);

                await _authRepository.CreateAsync(userAuth);

                await _emailService.SendUserFirstAccessContentAsync(new UserFirstAccessEmailRequest(user.FullName, user.Email, tempPassword));
            }
            catch
            {
                throw;
            }

            response.Data = user.Id;

            return response;
        }

        public async Task<Response> UpdateAsync(int id, UserUpdateRequest updateRequest)
        {
            var response = new Response();

            //if (_userInfo.Role == null)
            //{
            //    response.AddMessage("Usuário sem permissão.");
            //    return response;
            //}

            var user = await this._userRepository.GetByIdAsync(id);

            user.FullName = updateRequest.FullName ?? user.FullName;
            user.Email = updateRequest.Email ?? user.Email;
            user.BirthDate = updateRequest.BirthDate ?? user.BirthDate;
            user.ProfileId = updateRequest.ProfileId;
            user.UpdateDate = DateTime.Now;
            user.UpdateUser = _userInfo.Upn;

            await _userRepository.UpdateAsync(user);

            return response;
        }

        public async Task<Response> ChangeActiveAsync(bool isActive, int id)
        {
            var response = new Response();

            //if (_userInfo.Role == null)
            //{
            //    response.AddMessage("Usuário sem permissão.");
            //    return response;
            //}

            var user = await this._userRepository.GetByIdAsync(id);

            user.IsActive = isActive;
            user.UpdateDate = DateTime.Now;
            user.UpdateUser = _userInfo.Upn;

            await this._userRepository.UpdateAsync(user);

            return response;
        }

    }
}
