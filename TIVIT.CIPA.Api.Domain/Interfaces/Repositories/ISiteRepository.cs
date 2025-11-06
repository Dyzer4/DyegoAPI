using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Repositories
{
    public interface ISiteRepository
    {
        Task<Site> GetByIdAsync(int id);
        Task<IEnumerable<Site>> GetAllAsync();
        Task<IEnumerable<Site>> GetActiveAsync();
    }
}

