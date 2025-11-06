using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Model.Responses;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Business
{
    public interface ISiteBusiness
    {
        Task<Response<SiteResponse>> GetByIdAsync(int id);
        Task<Response<IEnumerable<SiteResponse>>> GetAllAsync();
        Task<Response<IEnumerable<SiteResponse>>> GetActiveAsync();
    }
}
