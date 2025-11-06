
using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Repositories
{
    public interface IVoterRepository
    {
        Task<Voter> GetByIdAsync(int id);
        Task<IEnumerable<Voter>> GetByElectionIdAsync(int electionId);
        Task UpdateAsync(Voter voter);
        Task CreateAsync(Voter voter);
        bool ExistsById(int id);
    }
}


