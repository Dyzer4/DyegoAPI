using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Interfaces.Repositories
{
    public interface IProfileRepository
    {
        Task<Profile> GetByIdAsync(int id);
        Task<IEnumerable<Profile>> GetAllAsync();
    }
}
