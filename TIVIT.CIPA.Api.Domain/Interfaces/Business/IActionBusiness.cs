using TIVIT.CIPA.Api.Domain.Model.Responses;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Business
{
    public interface IActionBusiness
    {
        Task<Response<IEnumerable<ActionResponse>>> GetByRoleIdAsync(int profileId);
    }
}
