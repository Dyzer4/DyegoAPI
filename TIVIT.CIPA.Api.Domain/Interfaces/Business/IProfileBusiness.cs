using TIVIT.CIPA.Api.Domain.Model.Responses;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Business
{
    public interface IProfileBusiness
    {
        Task<Response<IEnumerable<ProfileResponse>>> GetAllAsync();
    }
}
