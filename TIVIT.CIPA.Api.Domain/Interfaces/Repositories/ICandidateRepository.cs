
using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Repositories
{
    public interface ICandidateRepository
    {
        Task<Candidate> GetByIdAsync(int id);
        Task<IEnumerable<Candidate>> GetByElectionIdAsync(int electionId);
        Task<IEnumerable<Candidate>> SearchAsync(string name, int electionId, int? siteId = null, string? registry = null, string? departament = null);
        Task UpdateAsync(Candidate candidate);
        Task CreateAsync(Candidate candidate);
        bool ExistsById(int id);
    }
}

