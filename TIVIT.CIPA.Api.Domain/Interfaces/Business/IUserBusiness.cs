using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Model.Responses;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Business
{
    public interface IUserBusiness
    {
        Task<IEnumerable<User>> GetByActiveAsync(bool active);
        Task<Response<UserDetailResponse>> GetByIdAsync(int id);
        Task<Response<int>> CreateAsync(UserCreateRequest createRequest);
        Task<Response> UpdateAsync(int id, UserUpdateRequest updateRequest);
        Task<Response> ChangeActiveAsync(bool isActive, int id);
    }
}
