using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Model.Responses;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Business
{
    public interface ICandidateBusiness
    {
        Task<Response<CandidateDetailResponse>> GetByIdAsync(int id);
        Task<Response<IEnumerable<CandidateDetailResponse>>> GetByElectionIdAsync(int electionId);
        Task<Response<IEnumerable<CandidateResumeResponse>>> SearchCandidateAsync(string name, int electionId, int? siteId = null, string? corporateId = null, string? department = null);
        Task<Response<int>> CreateAsync(CandidateCreateRequest createRequest);
        Task<Response> UpdateAsync(int id, CandidateUpdateRequest updateRequest);
        Task<Response> ChangeActiveAsync(bool isActive, int id);
    }
}
