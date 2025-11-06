using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Repositories
{
    public interface IElectionRepository
    {
        Task<Election> GetByIdAsync(int id);
        Task<IEnumerable<Election>> GetAllAsync();
        Task UpdateAsync(Election election);
        Task CreateAsync(Election election);
        bool ExistsActiveByName(string name, int? ignoreId = null);
        bool ExistsById(int id);
    }
}

