
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Model.Responses;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Business
{
    public interface IElectionBusiness
    {
        Task<Response<ElectionResponse>> GetByIdAsync(int id);
        Task<Response<int>> CreateAsync(ElectionCreateRequest createRequest);
        Task<Response> UpdateAsync(int id, ElectionUpdateRequest updateRequest);
        Task<Response> ChangeActiveAsync(bool isActive, int id);
    }
}
