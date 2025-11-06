
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Model.Responses;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Business
{
    public interface IVoterBusiness
    {
        Task<Response<VoterDetailResponse>> GetByIdAsync(int id);
        Task<Response<IEnumerable<VoterResumeResponse>>> GetByElectionIdAsync(int electionId);
        Task<Response<int>> CreateAsync(VoterCreateRequest createRequest);
        Task<Response> UpdateAsync(int id, VoterUpdateRequest updateRequest);
        Task<Response> ChangeActiveAsync(bool isActive, int id);

        Response<byte[]> GetSyncTemplate();
        Task<Response> SyncByFileAsync(MemoryStream syncFile, int electionId);
    }
}
