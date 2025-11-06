using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Model.Results;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Repositories
{
    public interface IElectionSiteRepository
    {
        Task<IEnumerable<ElectionSite>> GetByElectionId(int electionId);
        Task<IEnumerable<Site>> GetSitesByElectionIdAsync(int electionId);
        Task AddRangeAsync(IEnumerable<ElectionSite> electionSites);
        Task RemoveRangeAsync(IEnumerable<ElectionSite> electionSites);
    }
}
